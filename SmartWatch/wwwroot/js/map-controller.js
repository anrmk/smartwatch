class MapController {
    constructor(options, callback = (target) => { }) {
        this.options = $.extend({
            'controller': '/',
            'mapContainer': 'map-container',
        }, options);

        this.markers = { devices: {}, onchange: async () => { /*this._showDeviceList();*/ } }; //стэк маркеров
        this.historyLine = null; //стек исторической линии на карте

        this.hubConnection = null; //соединение с хабом signalR

        this._mapInit();
        this._hubInit();
    }

    _mapInit() {
        this.map = L.map(this.options.mapContainer).setView([43.25654, 76.92848], 13);

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
            let marker;
            let latlng = L.latLng(x.latitude, x.longitude, x.altitude);

            if (this.markers.devices[x.deviceId] !== undefined) {
                marker = this.markers.devices[x.deviceId];
                marker.setLatLng(latlng).update();
            } else {
                marker = new L.marker(latlng, { 'draggable': false, 'riseOnHover': true, 'data': x });
                marker.addTo(this.map);
                this.markers.devices[x.deviceId] = marker;

                this.markers.onchange();
            }
        });
    }

    ///Показать линию перемещения устройства
    showHistory(id) {
        if (this.historyLine !== null)
            this.map.removeLayer(this.historyLine);

        $.ajax(`${this.options.controller}/${id}/locations`).done((data, status, jqXHR) => {
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
            this.map.setView(marker.getLatLng(), 18); //фокус на маркер
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