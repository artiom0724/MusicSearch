
$('img#loadingArtists').hide();
$('img#loadingAlbums').hide();
$('img#loadingTracks').hide();
var pageArtists = 1;
var _inCallbackArtists = false;
var pageAlbums = 1;
var _inCallbackAlbums = false;
var pageTracks = 1;
var _inCallbackTracks = false;

function addArtists() {
    if (pageArtists > 0 && !_inCallbackArtists) {
        _inCallbackArtists = true;
        pageArtists++;
        $('img#loadingArtists').show();
        $.ajax({
            type: 'GET',
            url: '/Home/AddArtists' + "?reqest=" + document.getElementById("myreqest").innerText + "&numPage=" + pageArtists.toString(),
            success: function (data, textstatus) {
                if (data != '') {
                    $("div#scrolListArtists").append(data);
                }
                else {
                    pageArtists = 1;
                }
                $('img#loadingArtists').hide();
                _inCallbackArtists = false;
            }
        });
    }
}

function addAlbums() {
    if (pageAlbums > 0 && !_inCallbackAlbums) {
        _inCallbackAlbums = true;
        pageAlbums++;
        $('img#loadingAlbums').show();
        $.ajax({
            type: 'GET',
            url: '/Home/AddAlbums' + "?reqest=" + document.getElementById("myreqest").innerText + "&numPage=" + pageAlbums.toString(),
            success: function (data, textstatus) {
                if (data != '') {
                    $("div#scrolListAlbums").append(data);
                }
                else {
                    pageAlbums = 1;
                }
                $('img#loadingAlbums').hide();
                _inCallbackAlbums = false;
            }
        });
    }
}

function addTracks() {
    if (pageTracks > 0 && !_inCallbackTracks) {
        _inCallbackTracks = true;
        $('img#loadingTracks').show();
        pageTracks++;
        $.ajax({
            type: 'GET',
            url: '/Home/AddTracks' + "?reqest=" + document.getElementById("myreqest").innerText + "&numPage=" + pageTracks.toString(),
            success: function (data, textstatus) {
                if (data != '') {
                    $("div#scrolListTracks").append(data);
                }
                else {
                    pageTracks = 1;
                }
                $('img#loadingTracks').hide();
                _inCallbackTracks = false;
            }
        });
    }
}