function calculator() {
    var dailyCost = $('input[name=dailyCost]').val();
    var leaseDays = $('input[name=leaseDays]').val();
    var dateStart = $('input[Name=dateStart]').val();
    

    //alert(dailyCost);
    //alert(leaseDays);
    //alert(dateStart);

    if (!isNaN(leaseDays)) {

        var result = dailyCost * leaseDays;
        $('#resultErrorLbl').hide();
        $('#dailyCostResult').val(result);
    }
    else
        $('#resultErrorLbl').show();
}

function order()
{
    var dateStart = $('input[name=dateStart]').val();
    var leaseDays = $('input[name=leaseDays]').val();
    var carId = $('input[name=carId]').val();

    var url = '/lease/order?carId=' + carId + "&dateStart=" + dateStart + "&leaseDays=" + leaseDays;
    location.replace(url);
    

}

