function TaxonViewModel(taxonId, family, genus, species) {
    var self = this;
    self.taxonId = taxonId;
    self.family = family;
    self.genus = genus;
    self.species = species;

    self.unknownGrains = ko.observableArray([]);
    self.referenceSlides = ko.observableArray([]);
    self.taxonomicStatus = ko.observable();

    self.updateUnknownGrains = function() {
        $.ajax({
            url: "/Taxon/ListUserSubmissions?p=1&pageSize=20&taxonId=" + self.taxonId,
            cache: false,
            success: function(jsondata) {
                ko.utils.arrayForEach(jsondata, function(grain){
                    self.unknownGrains.push(grain);
                });
            }
        })
    }

    self.updateReferenceSlides = function() {
        $.ajax({
            url: "/Taxon/ListReferenceMaterial?p=1&pageSize=20&taxonId=" + self.taxonId,
            cache: false,
            success: function(jsondata) {
                ko.utils.arrayForEach(jsondata, function(slide){
                    self.referenceSlides.push(slide);
                });
            }
        })
    }

    self.updateTaxonomicStatus = function() {
        $.ajax({
            url: "/api/v1/backbone/match?family=" + self.family + "&genus=" + self.genus + "&species=" + self.species,
            cache: false,
            success: function(status) {
                if (status.status == 0) status.status = "Confirmed Name";
                if (status.status == 1) status.status = "Synonym";
                if (status.status == 2) status.status = "Doubtful";
                if (status.status == 3) status.status = "Misapplied";
                self.taxonomicStatus(status);
            }
        })
    }

    self.loadData = function() {
        self.updateReferenceSlides();
        self.updateTaxonomicStatus();
        self.updateUnknownGrains();
        console.log('updating');
    }
}