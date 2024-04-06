
$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#DaySheetLink").addClass("active");
    $("#DaySheetLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const txtDate = $('input[name="Date"]');
    txtDate.val(todayDate);
    const CashSale = $('input[name="CashSale"]');
    const CreditSale = $('input[name="CreditSale"]');
    const OpnCashBal = $('input[name="OpnCashBal"]');
    const OpnBankBal = $('input[name="OpnBankBal"]');
    const ClosingCashBal = $('input[name="ClosingCashBal"]');
    const ClosingBankBal = $('input[name="ClosingBankBal"]');
    //-------------------------------------DaySheet Report------------------------------------------------//
    $('#btnView').on('click', function () {
        if (!txtDate.val()) {
            toastr.error('Date Is Required.');
            return;
        } else {
            $('#loader').hide();
            $.ajax({
                url: '/Reports/GetDaySheet?Date=' + txtDate.val() + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#loader').hide();
                    if (result.ResponseCode == 302) {
                        console.log(result);
                        $('.hdnDiv').show();
                        CashSale.val(result.DaySheet.CashSale);
                        CreditSale.val(result.DaySheet.CreditSale);
                        OpnCashBal.val(result.DaySheet.OpeningCashBal);
                        OpnBankBal.val(result.DaySheet.OpeningBankBal);
                        ClosingCashBal.val(result.DaySheet.ClosingCashBal);
                        ClosingBankBal.val(result.DaySheet.ClosingBankBal);
                        if (result.DaySheet.Purchases.length > 0) {

                            $('.PurchaseTable').empty();
                            var html = '';
                            html += '<table class="table table-bordered table-hover text-center mt-2 PurchaseTable" style="width:100%">';
                            html += '<thead>'
                            html += '<tr>'
                            html += '<th colspan="6">Purchase</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th>Trxn No</th>'
                            html += '<th>Narration</th>'
                            html += '<th>Item</th>'
                            html += '<th>Qty</th>'
                            html += '<th>Rate</th>'
                            html += '<th>Amount</th>'
                            html += '</tr>'
                            html += '</thead>'
                            html += '<tbody>';
                            $.each(result.DaySheet.Purchases, function (key, purchaseOdr) {
                                var firstRow = true;
                                $.each(purchaseOdr.PurchaseTransactions, function (key, purchaseTrn) {
                                    html += '<tr>';
                                    if (firstRow) {
                                        html += '<td>' + purchaseOdr.TransactionNo + '</td>';
                                        html += '<td>' + purchaseOdr.PartyName + '</td>';
                                        firstRow = false;
                                    } else {
                                        html += '<td>-</td>';
                                        html += '<td>-</td>';
                                    }
                                    html += '<td>' + purchaseTrn.ProductName + '</td>';
                                    html += '<td>' + purchaseTrn.AlternateQuantity + '</td>';
                                    html += '<td>' + purchaseTrn.Rate + '</td>';
                                    html += '<td>' + purchaseTrn.Amount + '</td>';
                                    html += '</tr>';
                                })
                            })
                            html += ' </tbody>';
                            html += '</table >';
                            $('.tblPurchase').html(html);
                        }
                        if (result.DaySheet.CreditSales.length > 0) {
                            $('.CreditSaleTable').empty();
                            var html = '';
                            html += '<table class="table table-bordered table-hover text-center mt-2 CreditSaleTable" style="width:100%">';
                            html += '<thead>'
                            html += '<tr>'
                            html += '<th colspan="6">Credit Sales</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th>Trxn No</th>'
                            html += '<th>Narration</th>'
                            html += '<th>Item</th>'
                            html += '<th>Qty</th>'
                            html += '<th>Rate</th>'
                            html += '<th>Amount</th>'
                            html += '</tr>'
                            html += '</thead>'
                            html += '<tbody>';
                            $.each(result.DaySheet.CreditSales, function (key, CreditSaleOdr) {
                                var firstRow = true;
                                $.each(CreditSaleOdr.SalesTransactions, function (key, CreditSaleTrn) {
                                    html += '<tr>';
                                    if (firstRow) {
                                        html += '<td>' + CreditSaleOdr.TransactionNo + '</td>';
                                        html += '<td>' + CreditSaleOdr.CustomerName + '</td>';
                                        firstRow = false;
                                    } else {
                                        html += '<td>-</td>';
                                        html += '<td>-</td>';
                                    }
                                    html += '<td>' + CreditSaleTrn.ProductName + '</td>';
                                    html += '<td>' + CreditSaleTrn.Quantity + '</td>';
                                    html += '<td>' + CreditSaleTrn.Rate + '</td>';
                                    html += '<td>' + CreditSaleTrn.Amount + '</td>';
                                    html += '</tr>';
                                })
                            })
                            html += ' </tbody>';
                            html += '</table >';
                            $('.tblCreditSale').html(html);
                        }
                        if (result.DaySheet.CashSales.length > 0) {
                            $('.CashSaleTable').empty();
                            var html = '';
                            html += '<table class="table table-bordered table-hover text-center mt-2 CashSaleTable" style="width:100%">';
                            html += '<thead>'
                            html += '<tr>'
                            html += '<th colspan="6">Cash Sales</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th>Trxn No</th>'
                            html += '<th>Narration</th>'
                            html += '<th>Item</th>'
                            html += '<th>Qty</th>'
                            html += '<th>Rate</th>'
                            html += '<th>Amount</th>'
                            html += '</tr>'
                            html += '</thead>'
                            html += '<tbody>';
                            $.each(result.DaySheet.CashSales, function (key, CashSaleOdr) {
                                var firstRow = true;
                                $.each(CashSaleOdr.SalesTransactions, function (key, CashSaleTrn) {
                                    html += '<tr>';
                                    if (firstRow) {
                                        html += '<td>' + CashSaleOdr.TransactionNo + '</td>';
                                        html += '<td>' + CashSaleOdr.CustomerName + '</td>';
                                        firstRow = false;
                                    } else {
                                        html += '<td>-</td>';
                                        html += '<td>-</td>';
                                    }
                                    html += '<td>' + CashSaleTrn.ProductName + '</td>';
                                    html += '<td>' + CashSaleTrn.Quantity + '</td>';
                                    html += '<td>' + CashSaleTrn.Rate + '</td>';
                                    html += '<td>' + CashSaleTrn.Amount + '</td>';
                                    html += '</tr>';
                                })
                            })
                            html += ' </tbody>';
                            html += '</table >';
                            $('.tblCashSale').html(html);
                        }
                        if (result.DaySheet.Receipts.length > 0) {
                            $('.ReciptTable').empty();
                            var html = '';
                            html += '<table class="table table-bordered table-hover text-center mt-2 ReciptTable" style="width:100%">';
                            html += '<thead>'
                            html += '<tr>'
                            html += '<th colspan="5">Receipt</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th>Trxn No</th>'
                            html += '<th>Narration</th>'
                            html += '<th>Cash/Bank</th>'
                            html += '<th>From Acc</th>'
                            html += '<th>Amount</th>'
                            html += '</tr>'
                            html += '</thead>'
                            html += '<tbody>';
                            $.each(result.DaySheet.Receipts, function (key, Receipt) {
                                html += '<tr>';
                                html += '<td>' + Receipt.VouvherNo + '</td>';
                                html += '<td>' + Receipt.Narration + '</td>';
                                html += '<td>' + Receipt.CashBank + '</td>';
                                html += '<td>' + Receipt.FromAcc + '</td>';
                                html += '<td>' + Receipt.Amount + '</td>';
                                html += '</tr>';
                            })
                            html += ' </tbody>';
                            html += '</table >';
                            $('.tblRecive').html(html);
                        }
                        if (result.DaySheet.Payments.length > 0) {
                            $('.PaymentTable').empty();
                            var html = '';
                            html += '<table class="table table-bordered table-hover text-center mt-2 PaymentTable" style="width:100%">';
                            html += '<thead>'
                            html += '<tr>'
                            html += '<th colspan="5">Payments</th>'
                            html += '</tr>'
                            html += '<tr>'
                            html += '<th>Trxn No</th>'
                            html += '<th>Narration</th>'
                            html += '<th>Cash/Bank</th>'
                            html += '<th>To Acc</th>'
                            html += '<th>Amount</th>'
                            html += '</tr>'
                            html += '</thead>'
                            html += '<tbody>';
                            $.each(result.DaySheet.Payments, function (key, Payment) {
                                html += '<tr>';
                                html += '<td>' + Payment.VouvherNo + '</td>';
                                html += '<td>' + Payment.Narration + '</td>';
                                html += '<td>' + Payment.CashBank + '</td>';
                                html += '<td>' + Payment.ToAcc + '</td>';
                                html += '<td>' + Payment.Amount + '</td>';
                                html += '</tr>';
                            })
                            html += ' </tbody>';
                            html += '</table >';
                            $('.tblPayment').html(html);
                        }
                    }
                },
                error: function (errormessage) {
                    $('#loader').hide();
                    Swal.fire(
                        'Error!',
                        'An error occurred',
                        'error'
                    );
                }
            });
        }
    })
    $('#BtnPrint').on('click', function () {
        var url = '/Print/DaySheetPrint?Date=' + txtDate.val() + '';
        window.open(url, '_blank');
    });
})