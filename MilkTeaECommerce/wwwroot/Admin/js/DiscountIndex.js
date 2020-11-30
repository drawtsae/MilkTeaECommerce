﻿$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/admin/discounts/getall'
        },
        "columns": [
            { "data": "name" },
            { "data": "dateStart" },
            { "data": "dateEnd" },
            { "data": "timeuselimit" },
            { "data": "timeused"},
            { "data": "per" },
            { "data": "max" },
            { "data": "code" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Admin/Discounts/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>

                                <a onClick=Delete("/Admin/Discounts/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>

                                <a href="#" data-target="#Detail" data-toggle="modal" data-id="${data}" 
                                class="btn btn-success" style="font-size:small">Details</a>

                                </a>
                            </div>  

                            
                           `
                }
            }


        ]

    });
});

$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idDiscount = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Admin/Discounts/Details/' + idDiscount,
        success: function (data) {
            console.log(data.idDiscount);
            modal.find('#Id').val(data.id);
            modal.find('#Name').val(data.name);
            modal.find('#Description').val(data.des);
            modal.find('#DateStart').val(data.dateStart);
            modal.find('#DateExpired').val(data.dateEnd);
            modal.find('#TimesUsed').val(data.timeUsed);
            modal.find('#TimesUseLimit').val(data.timeuselimit);
            modal.find('#PercentDiscount').val(data.per);
            modal.find('#MaxDiscount').val(data.max);
            modal.find('#Code').val(data.code);
            modal.find('#CategoryDiscount').val(data.cate);
            modal.find('#DeliveryDiscount').val(data.deli);
            modal.find('#ProductDiscount').val(data.prod);
        }
    })
})
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}

