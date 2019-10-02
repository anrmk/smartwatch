class MapController {
    constructor(options, callback = (target) => { }) {
        this.options = $.extend({
            'controller': '/',
            'mapContainer': 'map-container',
            'center': [43.25654, 76.92848],
            'zoom': 13,
        }, options);

        this.markers = { devices: {}, circles: {}, onchange: async () => { /*this._showDeviceList();*/ } }; //стэк маркеров
        this.historyLine = null; //стек исторической линии на карте

        this.hubConnection = null; //соединение с хабом signalR

        this._mapInit();
        this._hubInit();

        callback(this);
    }

    _mapInit() {
        this.map = L.map(this.options.mapContainer).setView(this.options.center, this.options.zoom);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: ''
        }).addTo(this.map);
    }

    _hubInit() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl("/deviceLocationHub").build();
        this.hubConnection.on("changeDevicesLocation", (data) => {
            this._changeDevicesLocation(data);
        });
        this.hubConnection.onclose(async () => { setTimeout(() => this._hubStartConnection(), 10000); });
        this._hubStartConnection();
    }

    async _hubStartConnection() {
        await this.hubConnection.start().then(() => {

        }).catch(err => {
            setTimeout(() => this._hubStartConnection(), 5000);
            return console.log(err);
        });
    }

    ///Изменить позиционирование устройства
    _changeDevicesLocation(data) {
        data.map((x, idx) => {
            let marker, circle;
            let latlng = L.latLng(x.latitude, x.longitude, x.altitude);

            if (this.markers.devices[x.deviceId] !== undefined) {
                marker = this.markers.devices[x.deviceId];
                marker.setLatLng(latlng).update();

                circle = this.markers.circles[x.deviceId];
                circle.setRadius(x.accuracy).setLatLng(latlng);

            } else {
                marker = new L.marker(latlng, { 'draggable': false, 'riseOnHover': true, 'data': x });
                marker.addTo(this.map);
                this.markers.devices[x.deviceId] = marker;

                circle = L.circle(latlng, { 'radius': x.accuracy }).addTo(this.map);
                this.markers.circles[x.deviceId] = circle;

                this.markers.onchange();
            }
        });
    }

    ///Показать линию перемещения устройства
    showHistory(id) {
        if (this.historyLine !== null)
            this.map.removeLayer(this.historyLine);

        var startDate = $('[name=StartFrom]').val();
        var endDate = $('[name=EndTill]').val();

        $.ajax({
            url: `${this.options.controller}/${id}/locations`,
            data: { start: startDate, end: endDate }
        }).done((data, status, jqXHR) => {
            this.focus(id);
            this.pointList = data.map((x, idx) => {
                return new L.latLng(x.latitude, x.longitude);
            });
            if (this.pointList.length > 0) {
                this.historyLine = new L.Polyline(this.pointList, { 'color': 'red', 'weight': 3, 'opacity': 0.5, 'smoothFactor': 1 });
                this.historyLine.addTo(this.map);
            }
        });
    }

    focus(id) {
        var marker = this.markers.devices[id];
        if (marker != null) {
            var options = marker.options.data;
            marker.bindPopup(`<strong>${options.deviceName}</strong>`).openPopup();
            this.map.panTo(marker.getLatLng()); //фокус на маркер
        }
    }

    ///Показать информацию по устройству
    showInfo(id, callback = (data, status, jqXHR) => { }) {
        $.ajax(`${this.options.controller}/device/${id}`).done((data, status, jqXHR) => {
            var marker = this.markers.devices[id];
            data.marker = marker != null ? marker.options.data : {};
            callback(data, status, jqXHR);
        });
    }
}