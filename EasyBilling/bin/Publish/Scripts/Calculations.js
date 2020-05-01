function calpcs() {
    var pcs = $('#pcs').val();
    var quantty = $('#quantty').val();
    var rate = $('#rate1').val();
    var txableRate = $('#txableRate').val();
    var disper = $('#disper').val();
    var dis = $('#discount').val();
    var subttl = $('#totalRate').val();
    var sngltx = $('#sngltaxes :selected').val();
    var multx = $('#taxes :selected').val();
    if ($("#sngltaxes").is(":visible")) {
        var tx = sngltx;
    } else {
        var tx = multx;
    }


    var pc = (pcs * 1).toFixed(0);
    var qnt = (quantty * 1).toFixed(3);
    var rt = (rate * 1).toFixed(2);
    var txrt = (txableRate * 1).toFixed(2);
    var dspr = (disper * 1).toFixed(2);
    var ds = (dis * 1).toFixed(2);
    var sbttl = (subttl * 1).toFixed(2);
    var ftaxes = (tx * 1).toFixed(2);

    var ttl = (pcs * rate).toFixed(2);
    var taxttl = ((pcs * rate) + ((ttl * tx) / 100)).toFixed(2);
    if (ds == 0.00) {

        ds = (taxttl * dspr) / 100;
    }
    var ttl1 = (taxttl - ds).toFixed(2);
    document.getElementById("pcs").value = pc.valueOf();
    document.getElementById("quantty").value = qnt.valueOf();
    document.getElementById("rate1").value = rt.valueOf();
    document.getElementById("txableRate").value = ttl.valueOf();
    document.getElementById("disper").value = dspr.valueOf();
    document.getElementById("discount").value = ds.valueOf();
    document.getElementById("totalRate").value = ttl1.valueOf();
}
function formation() {

    var pcs = $('#pcs').val();
    var quantty = $('#quantty').val();
    var rate = $('#rate1').val();
    var txableRate = $('#txableRate').val();
    var disper = $('#disper').val();
    var dis = $('#discount').val();
    var subttl = $('#totalRate').val();
    var sngltx = $('#sngltaxes :selected').val();
    var multx = $('#taxes :selected').val();
    if ($("#sngltaxes").is(":visible")) {
        var tx = sngltx;
    } else {
        var tx = multx;
    }



    var pc = (pcs * 1).toFixed(0);
    var qnt = (quantty * 1).toFixed(3);
    var rt = (rate * 1).toFixed(2);
    var txrt = (txableRate * 1).toFixed(2);
    var dspr = (disper * 1).toFixed(2);
    var ds = (dis * 1).toFixed(2);
    var sbttl = (subttl * 1).toFixed(2);
    var ftaxes = (tx * 1).toFixed(2);

    var ttl = (pcs * rate).toFixed(2);
    var taxttl = ((pcs * rate) + ((ttl * tx) / 100)).toFixed(2);
    if (ds == 0.00) {

        ds = (taxttl * dspr) / 100;
    }
    var ttl1 = (taxttl - ds).toFixed(2);
    document.getElementById("pcs").value = pc.valueOf();
    document.getElementById("quantty").value = qnt.valueOf();
    document.getElementById("rate1").value = rt.valueOf();
    document.getElementById("txableRate").value = ttl.valueOf();
    document.getElementById("disper").value = dspr.valueOf();
    document.getElementById("discount").value = ds.valueOf();
    document.getElementById("totalRate").value = ttl1.valueOf();

}
function taxformation(e) {

    var pcs = $('#pcs').val();
    var quantty = $('#quantty').val();
    var rate = $('#rate1').val();
    var txableRate = $('#txableRate').val();
    var taxes = e;

    var disper = $('#disper').val();
    var dis = $('#discount').val();
    var subttl = $('#totalRate').val();

    var pc = (pcs * 1).toFixed(0);
    var qnt = (quantty * 1).toFixed(3);
    var rt = (rate * 1).toFixed(2);
    var txrt = (txableRate * 1).toFixed(2);
    var ftaxes = (taxes * 1).toFixed(2);
    var dspr = (disper * 1).toFixed(2);
    var ds = (dis * 1).toFixed(2);
    var sbttl = (subttl * 1).toFixed(2);

    var ttl = (pcs * rate).toFixed(2);
    var tx = ((ttl * taxes) / 100).toFixed(2);
    var taxttl = ((pcs * rate) + ((ttl * taxes) / 100)).toFixed(2);

    if (ds == 0.00) {
        ds = (taxttl * dspr) / 100;
    }
    var ttl1 = (taxttl - ds).toFixed(2);
    document.getElementById("pcs").value = pc.valueOf();
    document.getElementById("quantty").value = qnt.valueOf();
    document.getElementById("rate1").value = rt.valueOf();
    document.getElementById("txableRate").value = ttl.valueOf();
    document.getElementById("disper").value = dspr.valueOf();
    document.getElementById("discount").value = ds.valueOf();
    document.getElementById("totalRate").value = ttl1.valueOf();

}
function formationfordiscount() {

    var pcs = $('#pcs').val();
    var quantty = $('#quantty').val();
    var rate = $('#rate1').val();
    var txableRate = $('#txableRate').val();
    var disper = $('#disper').val();
    var dis = $('#discount').val();
    var subttl = $('#totalRate').val();
    var sngltx = $('#sngltaxes :selected').val();
    var multx = $('#taxes :selected').val();
    if ($("#sngltaxes").is(":visible")) {
        var tx = sngltx;
    } else {
        var tx = multx;
    }

    if (tx == null || tx == 0.00 || tx == '') {
        tx = 0.00;
    }

    var pc = (pcs * 1).toFixed(0);
    var qnt = (quantty * 1).toFixed(3);
    var rt = (rate * 1).toFixed(2);
    var txrt = (txableRate * 1).toFixed(2);
    var tx1 = (tx * 1).toFixed(2);
    var dspr = (disper * 1).toFixed(2);
    var ds = (dis * 1).toFixed(2);
    var sbttl = (subttl * 1).toFixed(2);

    var ttl = (pcs * rate).toFixed(2);
    var newtax = ((pcs * rate) + (((pcs * rate) * tx1) / 100)).toFixed(2);
    ds = (newtax * dspr) / 100;

    var ttl1 = (newtax - ds).toFixed(2);
    document.getElementById("pcs").value = pc.valueOf();
    document.getElementById("quantty").value = qnt.valueOf();
    document.getElementById("rate1").value = rt.valueOf();
    document.getElementById("txableRate").value = ttl.valueOf();
    document.getElementById("disper").value = dspr.valueOf();
    document.getElementById("discount").value = ds.valueOf();
    document.getElementById("totalRate").value = ttl1.valueOf();

}