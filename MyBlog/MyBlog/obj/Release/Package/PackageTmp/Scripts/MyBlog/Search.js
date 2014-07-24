
$(document).ready(function () {
    $("#btnGlobalSearch").click(function () {
        var query = $("#txtSearchQuery").val();
        $.ajax({
            type: "GET",
            url: "/MyBlog/GlobalSearch",
            data: { query: query },
            success: OnGlobalSearchSuccess,
            error : OnError
        });
    });
});

function OnGlobalSearchSuccess(data)
{
    $("#divPostsList").empty().append(data);
}