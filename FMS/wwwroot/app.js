$(function () {
    $('a[data-page]').on('click', function (e) {
        e.preventDefault();
        var page = $(this).data('page');
        loadPage(page);
    });
    function loadPage(page) {
        debugger
       
        $.ajax({
            url: '/' + page,
            type: 'GET',
            dataType: 'html',
            success: function (data) {
                $('#app-content').html(data);
                $('#app-content').find('script').each(function () {
                    eval($(this).text());
                });
            },
            error: function () {
                alert('Error loading page.');
            }
        });
    }
});
