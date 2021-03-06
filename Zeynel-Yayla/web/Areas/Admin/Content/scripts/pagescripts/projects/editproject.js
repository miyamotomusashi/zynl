﻿$(function () {
    var status = $("#ProcessMessage").val();
    $("#imgloader").css("display", "none");
    if (status == "True" || status == "true")
        MessageBox("İşlem Başarıyla Tamamlandı", "info");
    else if (status == "False" || status == "false")
        MessageBox("İşlem Sırasında Bir Hata Oluştu.", "alert");


    var selval = $("#Language option:selected").val();
    
    if (selval == "") {
        $("#ProjectGroupId").attr("disabled", "disabled");
        $("#ProjectGroupId").empty().append($("<option></option>").val("").html("Proje Grubu Seçiniz..."));
    }

    

    $("#Language").change(function () {

        var val = $("#Language option:selected").val();
        if (val == "") { $("#ProjectGroupId").attr("disabled", true); }
        else {
            $("#imgloader").css("display", "inline-block");
            $.ajax({
                type: 'POST',
                url: '/Project/LoadGroup',
                data: '{lang:"' + val + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (result) {
                    $("#ProjectGroupId").empty().append($("<option></option>").val("").html("Proje Grubu Seçiniz..."));

                    $.each(result, function (i, item) {
                        $("#ProjectGroupId").append($("<option></option>").val(item.Value).html(item.Text));
                    });
                    $("#ProjectGroupId").removeAttr("disabled");
                    $("#imgloader").css("display", "none");
                },
                error: function () {

                }
            });


        }
             
      
    });

    
   
   
});
