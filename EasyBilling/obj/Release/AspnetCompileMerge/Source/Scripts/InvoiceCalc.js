var nm = 1;
var del = '';
var count = 1;
var delitm = '';
var list = [];
$(document).ready(function () {
   
    $('.prdtlsdiv').on('click', '.prdctprce,#prdctnm,.men-thumb-item', function () {
      
        var a = 1;
        var tkn = $(this).parents('div.prdtlsdiv').find('.prdcttkn').text();
        var maintotal = $(this).parents('div.prdtlsdiv').find('label.ttllvl').children('span#ttl').text();
        var maintax = $(this).parents('div.prdtlsdiv').find('.prdctaxtbl').text();
        var maindiscount = $(this).parents('div.prdtlsdiv').find('.prdcdistbl').text();
        var mainpcs = $(this).parents('div.prdtlsdiv').find('label.ttlpcs').children('span#ttlpcsspn').text();
        var grndttl = (parseFloat($('.grndftr').find('label.gndamtlbl').children('span.grndamt').text()) + parseFloat(maintotal)).toFixed(2);
        var grnddis = (parseFloat($('.grndftr').find('label.gnddislbl').children('span.grnddis').text()) + parseFloat(maindiscount)).toFixed(2);
        var grndtax = (parseFloat($('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text()) + parseFloat(maintax)).toFixed(2);
       
        $('.grndftr').find('label.gndamtlbl').children('span.grndamt').text(grndttl);
        $('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text(grndtax);
        $('.grndftr').find('label.gnddislbl').children('span.grnddis').text(grnddis);
        
        $('#tblcontainer table.nwdv').each(function (index, ele) {
            var i = index + 1;
       
                if ($('.remove' + i).find('.prdcttkntbl').text() == tkn) {
                    a = 2;

                    var total = (parseFloat($('.remove' + i).find('label.ttllvl').children('span#ttl').text()) + parseFloat(maintotal)).toFixed(2);
                    var pcs = (parseInt($('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text()) + parseInt(mainpcs)).toFixed(0);
                    $('.remove' + i).find('label.ttllvl').children('span#ttl').text(total);
                    $('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text(pcs);

                    var totaldiscnt = (parseFloat($('.remove' + i).find('label.prdcdistbl').text()) + parseFloat(maindiscount)).toFixed(2);
                    var totaltx = (parseFloat($('.remove' + i).find('label.prdctaxtbl').text()) + parseFloat(maintax)).toFixed(2);

                    $('.remove' + i).find('label.prdcdistbl').text(totaldiscnt);
                    $('.remove' + i).find('label.prdctaxtbl').text(totaltx);
                    return false;
                } else {
                    a = 3;
                }
        });
        
        if (list.length != 0 && (a == 1 || a == 3)) {
            jQuery.each(list, function(index, item) {
                delitm = item;
            });
            if ($(delitm).find('.prdcttkntbl').text() == tkn) {
                a = 2;

                var total = (parseFloat($('.remove' + i).find('label.ttllvl').children('span#ttl').text()) + parseFloat(maintotal)).toFixed(2);
                var pcs = (parseInt($('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text()) + parseInt(mainpcs)).toFixed(0);
                var totaldiscnt = (parseFloat($('.remove' + i).find('label.prdcdistbl').text()) + parseFloat(maindiscount)).toFixed(2);
                var totaltx = (parseFloat($('.remove' + i).find('label.prdctaxtbl').text()) + parseFloat(maintax)).toFixed(2);

                $('.remove' + i).find('label.ttllvl').children('span#ttl').text(total);
                $('.remove' + i).find('label.ttlpcs').children('span#ttlpcsspn').text(pcs);
                $('.remove' + i).find('label.prdcdistbl').text(totaldiscnt);
                $('.remove' + i).find('label.prdctaxtbl').text(totaltx);
                return false;
            } else {
                var $newRow = $(this).parents('div.prdtlsdiv').find('#tblprdcts').clone().removeAttr('id');
                $('.remove', $newRow).show();
                $('#tblcontainer').append($newRow.removeAttr('class').addClass(delitm).addClass('nwdv'));
                list.splice($.inArray(delitm, list), 1);
                del = '';
                delitm = '';

            }
        }
      
        else {
            if (a == 1 || a == 3) {
                var $newRow = $(this).parents('div.prdtlsdiv').find('#tblprdcts').clone().removeAttr('id');

                $('.remove', $newRow).show();
                $('#tblcontainer').append($newRow.removeAttr('class').addClass('remove' + nm).addClass('nwdv'));
                nm = nm + 1;
            }
        }

    });

    $('#tblcontainer').on('click', '.remove', function () {
      
        del = $(this).parents('table.nwdv').attr('class').split(' ')[0];
        jQuery.each(list, function (index, item) {
            if(item==del)
            {
                list.splice($.inArray(item, list), 1);
            }
        });
        list.push(del);
        list.sort();
        list.reverse();
        var removalamount = $(this).parents('table.nwdv').find('label.ttllvl').children('span#ttl').text();
        var removaldiscount = $(this).parents('table.nwdv').find('label.prdcdistbl').text();
        var removaltax = $(this).parents('table.nwdv').find('label.prdctaxtbl').text();
  
        var grndttl = (parseFloat($('.grndftr').find('label.gndamtlbl').children('span.grndamt').text()) - parseFloat(removalamount)).toFixed(2);
        var grnddis = (parseFloat($('.grndftr').find('label.gnddislbl').children('span.grnddis').text()) - parseFloat(removaldiscount)).toFixed(2);
        var grndtax = (parseFloat($('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text()) - parseFloat(removaltax)).toFixed(2);
        
        $('.grndftr').find('label.gndamtlbl').children('span.grndamt').text(grndttl);
        $('.grndftr').find('label.gndtaxlbl').children('span.grndtax').text(grndtax);
        $('.grndftr').find('label.gnddislbl').children('span.grnddis').text(grnddis);
        
        $(this).parents('table.nwdv').remove();

       
       
    });

});