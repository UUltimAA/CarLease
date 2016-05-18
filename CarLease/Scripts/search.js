function search() {
    var gear = $('select[name=gear]').val();
    var brand = $('select[name=brand]').val();
    var model = $('select[name=model]').val();
    var freeText = $('input[name=freeText]').val();
    var date = $('input[name=date]').val();

    $.ajax({
        url: "/Lease/Search",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            gear: gear,
            brand: brand,
            model: model,
            freeText: freeText,
            date: date
            
        }),
        dataType: "json",
        success: function (data) {
            var row = "";
            $.each(data, function (index, item) {
                row += "<tr><td>" + item.CarType.Brand + "</td><td> " + item.CarType.Model + "</td><td> " + item.CarType.DailyCost + "</td><td>" + item.CarType.DailyLateCost + "</td><td>" + item.CarType.Gear + "</td><td><button type='button' name='order' onclick='pickCar(" + item.Car.Id + ")'>Order Car</button></td></tr>";
            });
            $("#results").html(row);
        },
        error: function (result) {
            alert("Error");
        }
    });
}

function pickCar(carId) {
    location.replace("/lease/Calculator/calculator?CarId=" + carId);
}