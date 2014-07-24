

$(document).ready(function () {
    $("#btnDeletePost").click(function () {
        if (confirm("Are you sure you want to delete this Post?")) {
            var id = $("input[name='Id']").val();
            $.ajax({
                type: "POST",
                url: "/MyBlog/DeleteBlog",
                data: { id: id },
                success: onDeleteSuccess,
                error: OnError
            });
        }
        return false;
    });

    $("#btnSharePost").click(function () {

        var url = $(location).attr('href');
        var myBolgCaption = $("#title").text();
        var myBlogDescription = $("#description").text();

        FB.ui(
    {
        method: "feed",
        name: "Facebook Dialogs",
        link: url,
        picture: 'http://fbrell.com/f8.jpg',
        caption: myBolgCaption.toString(),
        description: myBlogDescription.toString()      
    },
      function (response) {
          if (response && response.post_id) {
              alert('Post was published.');
          } else {
              alert('Post was not published.');
          }
      }
    );
    });
});

function onDeleteSuccess(data)
{
    toastr.success(data);
    window.location.href = "/MyBlog/MyBlogs";
}