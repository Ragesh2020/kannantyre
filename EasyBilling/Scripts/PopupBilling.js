$(document).ready(function () {

    $('#submit').click(function () {
        var isAllValid = true;

        var list = [];
        var stocklist = [];
        var errorItemCount = 0;
       
        $('#tblcontainer table.nwdv').each(function (index, ele) {
            var i = index + 1;
            var orderItem = {
                
                Product_Token: $('.remove' + i).find('.prdcttkntbl').text(),
                Pieces: parseInt($('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text()).toFixed(0),
                Tax: parseFloat($('.remove' + i).find('label.prdctaxtbl').text()).toFixed(2),
                Discount: parseFloat($('.remove' + i).find('label.prdcdistbl').text()).toFixed(2),
                Sub_Total: parseFloat($('.remove' + i).find('label.ttllvl').children('span#ttl').text()).toFixed(2),
            }
            var stockItem = {
                
                Product_Token: $('.remove' + i).find('.prdcttkntbl').text(),
                Pieces: parseInt($('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text()).toFixed(0),
                Tax: parseFloat($('.remove' + i).find('label.prdctaxtbl').text()).toFixed(2),
                
                Sub_Total: parseFloat($('.remove' + i).find('label.ttllvl').children('span#ttl').text()).toFixed(2),
            }
         
            list.push(orderItem);
            stocklist.push(stockItem);
        });
        if (list.length == 0)
        {
            $('.errorstat').text("Atleast 1 item is required... Please try again...");
            $('#Errormodel').modal('toggle');
            $('#Errormodel').modal('show');
            isAllValid = false;
        }
        if (isAllValid) {

            var data = {
               
                Total_discount: parseFloat($('.grndftr').find('label.gnddislbl').children('span.grnddis').text()).toFixed(2),
                Total_tax: parseFloat($('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text()).toFixed(2),
             
                Total_amount: parseFloat($('.grndftr').find('label.gndamtlbl').children('span.grndamt').text()).toFixed(2),

                Billing_Details: list,
                Stockouts: stocklist
            }

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: 'http://localhost:8087/ProductsForSale/Bill',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                    
                        //alert(data.nmbr);
                        //$('.modal').find('label.gnddislbl').children('span.grnddis').text()
                        $('.bllno').text(data.nmbr);
                       
                        $('#prcdok').modal('toggle');
                        $('#prcdok').modal('show');
                        //here we will clear the form
                        list = [];
                        var a = null;
                        $.ajax({
                            type: 'GET',
                            url: 'http://localhost:8087/ProductsForSale/Search',

                        }).done(function (data) {
                            $("#searchResultnew").html(data);
                        });
                        $('.nwdv').remove();
                        $('.grndftr').find('label.gndamtlbl').children('span.grndamt').text('0.00');
                        $('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text('0.00');
                        $('.grndftr').find('label.gnddislbl').children('span.grnddis').text('0.00');

                        //$('#tblcontainer table.nwdv').each(function (index, ele) {
                        //    var i = index + 1;
                           
                           
                        //});


                        //$('#orderNo,#orderDate,#description').val('');
                        //$('#orderdetailsItems').empty();
                    }
                    else {
                        
                        $('.errorstat').text("Stock is not sufficient. Please check and try again...");
                        $('#Errormodel').modal('toggle');
                        $('#Errormodel').modal('show');
                        isAllValid = false;
                    }
                    $('#submit').val('Payment');
                },
                error: function (error) {
                    alert(error);
                    console.log(error);
                    $('#submit').val('Payment');
                }
            });
            
            
        }

    });

    $('.lnkclick').click(function () {
       
        window.open('http://localhost:8087/api/Billing/GetBillingListReport?Bllno=' + $('.bllno').text(), 'name', 'width=600,height=400');
    });
    
});