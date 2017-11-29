
//Product object
function Product(data) {
    this.Id = ko.observable(data.Id);
    this.Name = ko.observable(data.Name);
    this.Category = ko.observable(data.Category);
    this.Price = ko.observable(data.Price);
}
//start here
function GetProducts() {
    var self = this;
    self.products = ko.observableArray([]);
    self.Name = ko.observable();
    self.Category = ko.observable();
    self.Price = ko.observable();
    self.singleItem = ko.observable();
    self.search = ko.observable();

    $.getJSON('api/products', function (allData) {
        var mappedProducts = $.map(allData, function (item) { return new Product(item) });
        self.products(mappedProducts);
    });

    // Operations
    self.addData = function () {
        var id = 0;
        var lastItem = self.products()[self.products().length - 1];
        console.log(lastItem.Id());

        //set the data to the current data.
        var data = {
            Id: lastItem.Id() + 1,
            Name: self.Name(),
            Category: self.Category(),
            Price: self.Price()
        };
        self.products.push(new Product(data));
        self.save(data);

        //Clear the data
        self.Category("");
        self.Name("")
        self.Price("")
    };
    //deletes from the server
    self.delete = function (id) {

    }

    //removes from the front end
    self.remove = function (id) {

    }

    //Modifying the search button and UI to use ko
    self.findById = function () {
        self.ajaxCalls('api/products/' + self.search()).done(function (data) {
            $('#product').text(formatItem(data));
        });
        self.search("");
    }
    // Saves the data to the server
    self.save = function (data) {
        var uri = 'api/products';
        self.ajaxCalls(uri, 'POST', data);
    }

    function formatItem(item) {
        return item.Name + ': $' + item.Price;
    }

    //General ajax call
    self.ajaxCalls = function (uri, method, data) {
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('#product').text('Error: ' + err);
        });
    }
}

$(document).ready(function () {

    ko.applyBindings(new GetProducts());
});
