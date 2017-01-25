var map = L.map('map').setView([51.505, -0.09], 1);
L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
    attribution: 'Imagery © <a href="http://mapbox.com">Mapbox</a>',
    maxZoom: 18,
    id: 'mareep2000.onj49m55',
    accessToken: 'pk.eyJ1IjoibWFyZWVwMjAwMCIsImEiOiJjaWppeGUxdm8wMDQ3dmVtNHNhcHh0cHA1In0.OrAULrL8pJaL9N5WerUUDQ'
}).addTo(map);

var latitudes = document.getElementsByClassName('Latitude');
var longitudes = document.getElementsByClassName('Longitude');
for (var i = 0; i < latitudes.length; i++) {
    L.marker([latitudes[i].innerHTML, longitudes[i].innerHTML]).addTo(map);
}

var areaSelect = L.areaSelect({ width: 100, height: 100 });
areaSelect.addTo(map);
areaSelect.on("change", function () {
    var bounds = areaSelect.getBounds();
    console.log(bounds);
    $("#LatitudeHigh").val(bounds.getNorthEast().lat);
    $("#LatitudeLow").val(bounds.getSouthWest().lat);
    $("#LongitudeHigh").val(bounds.getNorthEast().lng);
    $("#LongitudeLow").val(bounds.getSouthWest().lng);
});
