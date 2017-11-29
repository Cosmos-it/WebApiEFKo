var uri = 'api/products';

function GetProducts() {
    var self = this;
    self.products = ko.observableArray([]);

    $.getJSON(uri, function (allData) {
        var mappedProducts = $.map(allData, function (item) { return new Product(item) });
        console.log(mappedProducts);
        self.products(mappedProducts);
    });
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

function Product(data) {
    this.Id = ko.observable(data.Id);
    this.Name = ko.observable(data.Name);
    this.Category = ko.observable(data.Category);
    this.Price = ko.observable(data.Price);
}

