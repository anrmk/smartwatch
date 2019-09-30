$(document).ready(() => {
    bsCustomFileInput.init();
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar, #content').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    $('.modal').on('hidden.bs.modal', function (e) {
        $(e.currentTarget).find('.modal-body').empty();
        $(e.currentTarget).find('.modal-title').empty();
    });
});