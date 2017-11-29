var uri = 'api/products';

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

    $.getJSON(uri, function (allData) {
        var mappedProducts = $.map(allData, function (item) { return new Product(item) });
        self.products(mappedProducts);
    });

    // Operations
    self.addData = function () {
        var id = 0;
        for (var i = 0; i < self.products().length; i++) {
            var value = self.products()[i];
            //if () {

            //}
            console.log(value.Id());
        }



        //set the data to the current data.
        var data = {
            Name: self.Name(),
            Category: self.Category(),
            Price: self.Price()
        };
        console.log(data);

        self.products.push(new Product(data));
        self.save(data);
        //Clear the data
        self.Category("");
        self.Name("")
        self.Price("")
    };


    // Saves the data to the server
    self.save = function (data) {
        $.ajax(uri, {
            data: JSON.stringify(data),
            type: "post",
            contentType: "application/json",
            success: function (result)
            { alert("Item with Id:" + result.Id + " saved to the server!") }
        });
    }

    //General ajax call
    self.ajaxCalls = function (uri, data) {
        $.ajax(uri, {
            data: JSON.stringify(data),
            type: "post",
            contentType: "application/json",
            success: function (result)
            { alert("Item with Id:" + result.Id + " saved to the server!") }
        });
    }
}


$(document).ready(function () {

    ko.applyBindings(new GetProducts());

});

//Gets an item
function formatItem(item) {
    return item.Name + ': $' + item.Price;
}

//Gets the data by id
function find() {

    var id = $('#prodId').val();

    $.getJSON(uri + '/' + id)
        .done(function (data) {
            $('#product').text(formatItem(data));
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#product').text('Error: ' + err);
        });
}

