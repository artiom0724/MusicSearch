$(function () {

    $('div#loading').hide();

    var page = 1;
    var _inCallback = false;
    var _isScroll = false;
    function loadItems() {
        if (page > 0 && !_inCallback) {
            _inCallback = true;
            page++;
            $('img#loading').show();

            $.ajax({
                type: 'GET',
                url: document.location.pathname+"?reqest="+ document.getElementById("myreqest").innerText + "&numPage="+ page.toString(),
                success: function (data, textstatus) {
                    if (data != '') {
                        $("#scrolList").append(data);
                    }
                    else {
                        page = 1;
                    }
                    _inCallback = false;
                    $("img#loading").hide();
                }
            });
        }
    }

    $(window).scroll(function () {
        if (_isScroll == false) {
            _isScroll = true;
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                loadItems();
            }
            _isScroll = false;
        }
    });
})