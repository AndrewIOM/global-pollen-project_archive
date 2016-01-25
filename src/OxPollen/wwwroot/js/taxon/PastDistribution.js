var neotomaId;
var points = [];
var yearOldest = 10000;
var yearYoungest = 1000;

//Map
var width = $('#neotoma-map').closest('.tab-content').width();
var height = 300;
var svg;
var domPoints;
var projection = d3.geo.equirectangular()
    .scale((width + 1) / 2 / Math.PI)
    .translate([width / 2, height / 2])
    .precision(.1);
var color = d3.scale.linear()
    .domain([yearYoungest, yearOldest])
    .range(["yellow", "#83296F"]);

$('document').ready(function () {

    //Error Handling
    neotomaId = $('#NeotomaId').val();
    if (neotomaId == 0) {
        var neotomaWarning = document.getElementById('neotoma-warning');
        var unavailableDiv = document.getElementById('neotoma-map-unavailable');
        var warningsContainer = document.getElementById('warnings-container');
        warningsContainer.style.display = '';
        neotomaWarning.style.display = '';
        unavailableDiv.style.display = '';
    } else {

        //Create year range slider
        var slider = document.getElementById('range');
        noUiSlider.create(slider, {
            start: [1, 10],
            margin: 1,
            connect: true,
            orientation: 'horizontal',
            behaviour: 'tap-drag',
            step: 1,
            tooltips: [false, wNumb({ postfix: 'kyBP' })],
            range: {
                'min': 1,
                'max': 50
            },
            pips: {
                mode: 'values',
                values: [1, 5, 10, 15, 20, 30, 40, 50],
                density: 1,
                stepped: true
            }
        });

        //Create Basemap
        svg = d3.select('#neotoma-map').append('svg')
        .attr('width', width)
        .attr('height', height);
        var path = d3.geo.path()
            .projection(projection);
        var g = svg.append("g");
        d3.json('/geoJSON/world-110m2.json', function (error, topology) {
            g.selectAll("path")
                .data(topojson.object(topology, topology.objects.countries)
                    .geometries)
                .enter()
                .append("path")
                .attr("d", path)
                .attr('fill', 'grey')
        });

        //Get NeotomaDB Data
        points = getNeotomaPoints();

        //Slider change event
        slider.noUiSlider.on('slide', function () {
            var value = slider.noUiSlider.get();
            yearOldest = value[1] * 1000;
            yearYoungest = value[0] * 1000;
            redrawPoints();
        });
    }
});

function redrawPoints() {
    domPoints.attr('display', 'none');
    domPoints.filter(function (d) { return d.youngest < yearOldest && yearYoungest < d.oldest }).attr('display', '');
}

var getNeotomaPoints = function () {
    var neotomaUri = "http://api.neotomadb.org/v1/data/datasets?callback=neotomaCallback&taxonids=" + neotomaId + "&ageof=taxon&ageold=" + 50000 + "&ageyoung=" + 1000;
    console.log('Updating Neotoma Points... ' + neotomaUri);
    $.ajax({
        url: neotomaUri,
        jsonp: false,
        jsonpCallback: 'neotomaCallback',
        cache: 'true',
        dataType: 'jsonp'
    });
}

function neotomaCallback(result) {
    if (result.success == 0) {
        console.log('neotoma: error');
        //Error
    } else {
        console.log('neotoma: success');
        points = [];
        for (var i = 0; i < result.data.length; i++) {
            var coord = {
                east: result.data[i].Site.LongitudeEast,
                north: result.data[i].Site.LatitudeNorth,
                youngest: result.data[i].AgeYoungest,
                oldest: result.data[i].AgeOldest
            };
            points.push(coord);
        }

        domPoints = svg.selectAll("circle")
            .data(points).enter()
            .append("circle")
            .attr("cx", function (d) { return projection([d.east, d.north])[0]; })
            .attr("cy", function (d) { return projection([d.east, d.north])[1]; })
            .attr("r", "1.5px")
            .attr("fill", function (d) { return color(d.youngest); });
    }
}

function ajaxHelper(uri, method, dataType, data) {
    return $.ajax({
        type: method,
        url: uri,
        dataType: dataType,
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXhr, textStatus, errorThrown) {
        console.log(errorThrown);
    });
}