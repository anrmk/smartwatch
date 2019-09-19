class MapController {
    constructor(options, callback = (target) => { }) {
        this.options = options || {};
        this.controller = this.options.controller;
        this.mapContainer = this.options.mapContainer || 'map-container';
        this.containerInfo = this.options.containerInfo || '.container-info';

        this.markers = { devices: {}, onchange: async () => { this._showDeviceList(); } }; //стэк маркеров
        this.historyLine = null; //стеэ исторической линии на карте

        this.hubConnection = null; //соединение с хабом signalR

        this._mapInit();
        this._hubInit();
    }

    _mapInit() {
        this.map = L.map(this.mapContainer).setView([43.25654, 76.92848], 13);

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
                //marker.on('click', () => { this._showDeviceInfo(x); });
                marker.addTo(this.map).bindPopup(`<strong>${x.deviceName}</strong>`).openPopup();
                this.markers.devices[x.deviceId] = marker;

                this.markers.onchange();
            }
        });
    }

    ///Показать список устройств
    async _showDeviceList() {
        var container = $(this.containerInfo).find('.device-list');
        if (container.length === 0) {
            container = $(`<div class='mt-3 device-list'>
                            <h5 class='m-3'>Список устройств:</h5>
                            <div class='device-list-body'></div>
                        </div>`);
            container.appendTo(this.containerInfo);
        }

        await $.each(this.markers.devices, (idx, x) => {
            let data = x.options.data;
            var item = container.find(`div.device-list-body div[name=${data.id}]`);
            if (item.length === 0) {
                let link = $(`<div class='list-group-item list-group-item-action' name='${data.id}'>
                                ${data.deviceName || "Не определен"} 
                                <span><a href=''><i class='fa fa-bullseye-pointer'></i></a></span>
                                <span class='d-block'>последние изменения: ${data.timestamp}</span>
                              </div>`).on('click', (e) => {
                        this.map.setView(x.getLatLng(), 13); //фокус на маркер
                        this._showDeviceInfo(data);
                        this._showDeviceLocationHistory(data.deviceId); //показать линию перемещения
                    });
                container.find('div.device-list-body').append(link);
            }
        });
    }

    ///Показать линию перемещения устройства
    _showDeviceLocationHistory(id) {
        if (this.historyLine !== null)
            this.map.removeLayer(this.historyLine);

        $.ajax(`${this.controller}/${id}/locations`).done((data, status, jqXHR) => {
            this.pointList = data.map((x, idx) => {
                return new L.latLng(x.latitude, x.longitude);
            });
            if (this.pointList.length > 0) {
                this.historyLine = new L.Polyline(this.pointList, { 'color': 'red', 'weight': 3, 'opacity': 0.5, 'smoothFactor': 1 });
                this.historyLine.addTo(this.map);
            }
        });
    }

    ///Показать информацию по устройству
    _showDeviceInfo(data) {
        var form = `<div class='mt-3 device-info'>
                        <h5 class='m-3'>Информация по устройству:</h5>
                        <div class='card'>
                          <div class='card-body device-info-body'>
                            <label class='d-block'>Наименование: <strong>${data.deviceName}</strong></label>
                            <label class='d-block'>Точность: <strong>${data.accuracy} м.</strong></label>
                            <label class='d-block'>Высота над уровнем моря: <strong>${data.altitude}</strong></label>
                            <label class='d-block'>Широта: <strong>${data.latitude}</strong></label>
                            <label class='d-block'>Долгота: <strong>${data.longitude}</strong></label>
                            <label class='d-block'>Скорость: <strong>${data.speed}</strong></label>
                            <label class='d-block'>Ложный провайдер: <strong>${(data.isFromMockProvider ? 'да' : 'нет')}</strong></label>
                            <label class='d-block'>Отметка времени: <strong>${data.timestamp}</strong></label>
                          </div>
                        </div>
                    </div>`;
        $(this.containerInfo).find('.device-info').remove();
        $(form).appendTo(this.containerInfo);
    }
}