$('document').ready(function () {

    var width = $('#continent-picker').width();
    var height = width * 0.70;

    var projection = d3.geo.mercator()
        .scale((width + 1) / 2 / Math.PI)
        .translate([width / 2, height * 0.75])
        .precision(.1);

    var svg = d3.select("#continent-picker").append("svg")
        .attr("width", width)
        .attr("height", height);
    var path = d3.geo.path()
        .projection(projection);
    var g = svg.append("g");

    d3.json("/geoJSON/world.geo.json", function (error, world) {
        console.log(error);
        console.log(world);

        var countries = topojson.feature(world, world.objects.world);

        //Define SUBREGIONS
        var subregions = [];

        //Africa
        subregions.push({ type: "FeatureCollection", name: "Eastern Africa", color: "#ffbb78", id: 14, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 14; }) });
        subregions.push({ type: "FeatureCollection", name: "Middle Africa", color: "#ffbb78", id: 17, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 17; }) });
        subregions.push({ type: "FeatureCollection", name: "Northern Africa", color: "#ffbb78", id: 15, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 15; }) });
        subregions.push({ type: "FeatureCollection", name: "Southern Africa", color: "#ffbb78", id: 18, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 18; }) });
        subregions.push({ type: "FeatureCollection", name: "Western Africa", color: "#ffbb78", id: 11, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 11; }) });

        //Americas
        subregions.push({ type: "FeatureCollection", name: "Caribbean", color: "#d62728", id: 29, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 29; }) });
        subregions.push({ type: "FeatureCollection", name: "Central America", color: "#d62728", id: 13, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 13; }) });
        subregions.push({ type: "FeatureCollection", name: "South America", color: "#d62728", id: 5, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 5; }) });
        subregions.push({ type: "FeatureCollection", name: "North America", color: "#1f77b4", id: 21, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 21; }) });

        //Asia
        subregions.push({ type: "FeatureCollection", name: "Central Asia", color: "#ffbb78", id: 143, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 143; }) });
        subregions.push({ type: "FeatureCollection", name: "Eastern Asia", color: "#ffbb78", id: 30, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 30; }) });
        subregions.push({ type: "FeatureCollection", name: "Southern Asia", color: "#ffbb78", id: 34, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 34; }) });
        subregions.push({ type: "FeatureCollection", name: "South-Eastern Asia", color: "#ffbb78", id: 35, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 35; }) });
        subregions.push({ type: "FeatureCollection", name: "Western Asia", color: "#ffbb78", id: 145, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 145; }) });

        //Europe
        subregions.push({ type: "FeatureCollection", name: "Eastern Europe", color: "#ff7f0e", id: 151, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 151; }) });
        subregions.push({ type: "FeatureCollection", name: "Northern Europe", color: "#ff7f0e", id: 154, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 154; }) });
        subregions.push({ type: "FeatureCollection", name: "Southern Europe", color: "#ff7f0e", id: 39, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 39; }) });
        subregions.push({ type: "FeatureCollection", name: "Western Europe", color: "#ff7f0e", id: 155, features: countries.features.filter(function (d) { return d.properties.SUBREGION == 155; }) });

        //Oceana
        subregions.push({ type: "FeatureCollection", name: "Oceania", color: "#aec7e8", id: 7, features: countries.features.filter(function (d) { return d.properties.REGION == 9; }) });

        var continent = g.selectAll(".continent").data(subregions);

        continent.enter().insert("path")
            .attr("class", "continent")
            .attr("d", path)
            .attr("id", function (d, i) { return d.id; })
            .attr("title", function (d, i) { return d.name; })
            .style("fill", function (d, i) { return d.color; })
            .style('stroke', 'white')
            .attr("stroke-width", 1);


        g.selectAll("path")
            .data(world.objects.world)
            .enter()
            .append("path")
            .attr("d", path)
            .attr("stroke", "blue")
            .attr("stroke-width", 2)
            .attr("fill", "none");

        continent.on('click', function (d, i) {
            document.getElementById('FocusRegion').value = d.name;
            continent.style('stroke', 'white');
            d3.select(this).style("stroke", "yellow");
        });

        continent.on("mouseover", function (d) {
            d3.select(this).style("fill", "black");
        })
        continent.on("mouseout", function (d) {
            d3.select(this).style("fill", function (d, i) { return d.color; });
        })

    });
});