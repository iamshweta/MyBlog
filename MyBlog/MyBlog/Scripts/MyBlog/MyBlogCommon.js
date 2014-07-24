
$(document).ready(function () {
    setPaginationEvents();
});

function setPaginationEvents()
{
    $("#linkNext").click(function () {
        var actionName = $("input[name='actionName']").val();
        var tag = $("input[name='Tag']").val();        
        var curr_pageNo = $("#hiddenPageIndex").val();
        var pageNo = Number(curr_pageNo) + 1;
        $.ajax({
            type: "GET",
            url: "/MyBlog/Pagination",
            data: { actionName: actionName, pageNo: pageNo , tag : tag},
            success: onPaginationSuccess,
            error: OnError
        });
    });

    $("#linkPrev").click(function () {
        var actionName = $("input[name='actionName']").val();
        var curr_pageNo = $("#hiddenPageIndex").val();
        var tag = $("input[name='Tag']").val();
        var pageNo = Number(curr_pageNo) - 1;
        $.ajax({
            type: "GET",
            url: "/MyBlog/Pagination",
            data: { actionName: actionName, pageNo: pageNo, tag: tag },
            success: onPaginationSuccess,
            error: OnError
        });
    });
}

function onPaginationSuccess(data)
{
    $("#divPostsList").empty().append(data);
    setPaginationEvents();
}

function OnSuccess(successMsg) {
    toastr.success(successMsg);
}

function OnError(xhr, ajaxOptions, thrownError) {
    toastr.error(thrownError.message);
}

function showError(msg) {
    toastr.error(msg);
}

