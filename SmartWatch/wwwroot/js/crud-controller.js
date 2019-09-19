class CrudController {
    constructor(options, callback = (form, data, status, self) => { }) {
        this.options = options || {};
        this.callback = callback;
        this._init();
    }
    _init() {
        $('.form').on('submit', (e) => {
            e.preventDefault();
            var $form = $(e.currentTarget);

            var opt = {
                'data': null,
                'type': $form.attr('method'),
                'url': $form.attr('action'),
                'type': $form.attr('method')
            }

            if ($form.attr('enctype') === 'multipart/form-data') {
                opt.contentType = false;
                opt.data = new FormData();

                $form.find('[name=files]').prop('files').forEach((source, index) => {
                    opt.data.append(source.name, source);
                });

            } else {
                opt.data = JSON.stringify($form.serializeJSON());
            }

            this._submit($form, opt);
            return false;
        });
    }

    _submit($form, opt) {
        let options = $.extend({
            'contentType': 'application/json; charset=utf-8',
            'processData': false,
            'beforeSend': (jqXHR, settings) => {
                if ($form.attr('method') === 'delete') {
                    if (!confirm('Вы действительно хотите удалить запись?')) {
                        jqXHR.abort();
                        return;
                    }
                }
                $form.find('fieldset').attr('disabled', 'disabled');
            },
            'complete': (jqXHR, settings) => {
                $form.find('fieldset').removeAttr('disabled');
            }
        }, opt);

        let validator = $($form).validate();
        if (validator.form()) {
            $.ajax(options).done((data, status, jqXHR) => {
                this.callback($form, data, status, this);
                return false;
            }).fail((jqXHR, status) => {
                if (console && console.log) {
                    console.log("Crud controller error: " + jqXHR.statusText)
                }
                this.callback($form, jqXHR.responseText, status, this);
                return false;
            });
        }
        return false;
    }
}