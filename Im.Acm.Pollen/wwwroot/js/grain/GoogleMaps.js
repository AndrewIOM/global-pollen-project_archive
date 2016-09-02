window.onload = function () {
    console.log("loading google maps");
    var latlng = new google.maps.LatLng(51.4975941, -0.0803232);
    var map = new google.maps.Map(document.getElementById('map'), {
        center: latlng,
        zoom: 5,
        mapTypeId: google.maps.MapTypeId.TERRAIN
    });

    var marker;
    function placeMarker(location) {
        if (marker) {
            marker.setPosition(location);
        } else {
            marker = new google.maps.Marker({
                position: location,
                map: map,
                title: "Pollen Sample Location",
                draggable: true
            });
            google.maps.event.addListener(marker, 'dragend', function (event) {
                updateLocationFormFields(event.latLng);
            });
        }
    }

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);
        updateLocationFormFields(event.latLng);
    });

    function updateLocationFormFields(latLng) {
        var lat = latLng.lat().toFixed(4);
        var lon = latLng.lng().toFixed(4);
        document.getElementById('Latitude').value = lat;
        document.getElementById('Longitude').value = lon;
    }
};