
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

    /** get all data **/
    $.getJSON('api/products', function (allData) {
        var mappedProducts = $.map(allData, function (item) { return new Product(item) });
        self.products(mappedProducts);
    });

    // Operations
    self.addData = function () {
        /** get the last product value **/
        var lastItemId = self.products()[self.products().length - 1];

        /** set the data to the current data. **/
        var data = {
            Id: lastItemId.Id() + 1, //Add 1 to the last id
            Name: self.Name(),
            Category: self.Category(),
            Price: self.Price()
        };

        /** save it to local array observable **/
        self.products.push(new Product(data));
        self.save(data);

        /** Clear the data **/
        self.Category("");
        self.Name("")
        self.Price("")
    };
    /**
    removes from the front end first.
     This will allow the user to think before 
     Going ahead with this process to avoid data removal.
    **/
    self.remove = function (dataObj) {
        var id = dataObj.Id();
        self.products.destroy(dataObj);
        self.delete(id);
    }

    //deletes from the server
    self.delete = function (id) {
        alert("Are you sure about this?");
        self.ajaxCalls('api/products/' + id, 'DELETE').done(function (data) {
            $('#success').text("Successfully deleted: " + formatItem(data));
        });
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

    //General ajax call
    self.ajaxCalls = function (uri, method, data) {
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('#product').text('Error: ' + errorThrown);
        });
    }

    function formatItem(item) {
        return item.Name + ': $' + item.Price;
    }
}

$(document).ready(function () {
    /** do binding **/
    ko.applyBindings(new GetProducts());
});
