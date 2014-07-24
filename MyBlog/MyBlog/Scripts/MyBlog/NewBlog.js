

$(document).ready(function () {
    $("#btnAddTag").click(function () {
        var text = $('#txtTagName').val();
        $('#txtTagName').val("");
        var tagCountSelector = $("#tagCount");
        
        if (tagCountSelector.length == 0) {
            var $hiddenTagCount = $('<input/>', { type: 'hidden', id: "tagCount", value: 0 });
            $hiddenTagCount.appendTo($("#divTagList"));
            tagCountSelector = $("#tagCount");
        }
        var tagIndex = tagCountSelector.val();
        if (text.length) {
            $('<li />', { html: text }).appendTo('ul.tagList');
            var $hiddenInput = $('<input/>', { type: 'hidden', id: text, value: text, name: 'TagNames[' + tagIndex + ']' });
            $hiddenInput.appendTo($("#divTagList"));
            tagCountSelector.val(Number(tagIndex) + 1);
        }
        else {
            showError("Please add Tag name");
        }
    });

    $("#btnPublish").click(function () {
        return validateForm();
    });

});

function validateForm()
{
    var isValid = true;
    var content = $("#divNewBlogText iframe").contents().find("body").text();
    if (content.length == 0)
    {
        showError("Please add some content");
        isValid = false;
    }
    var title = $("input[name='post.Title']").val();
    if (title.length == 0) {
        showError("Please add a Title");
        isValid = false;
    }
    var isTagAdded = $("#tagList li").length != 0;
    if (!isTagAdded)
    {
        showError("Please add atleast one Tag");
        isValid = false;
    }
    var description = $("input[name='post.Description']").val();
    if (description.length == 0) {
        showError("Please add some Description");
        isValid = false;
    }
    return isValid;
}