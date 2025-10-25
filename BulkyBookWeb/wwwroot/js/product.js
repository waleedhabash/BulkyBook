var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "coverType.name", "width": "15%" },
            {
                "data": "imageUrl", "render": function (data) {
                    return `
                 <div class="w-75 btn-group" role="group">
                           <img src="${data}" width="100%" style="border-radius:5px;border:1px solid #bbb9b9" />

                   </div>
                    `
                }, "width": "15%"
            },
            {
                "data": "id", "render": function (data) {
                    return `
                 <div class="w-75 btn-group" role="group">
                            <a class="btn btn-info mx-2" href="/Admin/Product/Upsert?id=${data}" ><i class="bi bi-pencil-square"></i> Edit</a>
                            <a class="btn btn-danger mx-2" onClick=Delete('/Admin/Product/Delete/${data}') ><i class="bi bi-trash"></i> Delete</a>

                   </div>
                    `
            }, "width": "15%" },
        ]
    });
    
}
function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        toastr.success(data.message)
                    } else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    });
}