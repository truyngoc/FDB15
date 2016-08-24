$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {
        
        $('#myModalContent').load(this.href, function () {
            

            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;
    });


    $("a[delete-modal]").on("click", function (e) {

        $('#deleteModalContent').load(this.href, function () {


            $('#deleteModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindFormDelete(this);
        });

        return false;
    });

    

});

function bindForm(dialog) {
    
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}


function bindFormDelete(dialog) {

    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#deleteModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#deleteModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}