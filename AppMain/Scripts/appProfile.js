

function idLookUp(idType, idNumber, idName, recordId, objectType, loaderId,idTypeName,statusCtrl) {

    $("#" + loaderId).slideDown();
    $.ajax({
        url: '/dblonline/Utilities/ValidateIdFromGvive?idType='+idType + "&idNumber=" + idNumber + "&idName=" + idName + "&recordId="+recordId + "&objectType="+objectType,
        type: "GET",
        async: true,
        processData: false,
        cache: false,
        success: function (response) {
        
             $("#" + loaderId).slideUp();
            if (!response.Error) {
                $('#' + statusCtrl).html('Validated-GVIVE');

                $("#photoIdVerificationDiv").html(response);
                $("#photoIdVerificationModal").modal();
          

            }
            else {
                bootbox.dialog(
                    {
                        title: "ID verification failed.",
                        message: "Manually verify ID?<br/>",
                        buttons: {
                            success: {
                                label: "YES",
                                className: "btn-success",
                                callback: function () {
                                    manualCardValidation(idNumber, recordId, objectType, idTypeName);
                                    return;
                                }
                            },
                            main: {
                                label: "No",
                                className: "btn-danger",
                                callback: function () {

                                }
                            }

                        }
                    });

            }
        },
        error: function (xhr, status, error) {
            alert('system error');
        }
    });
}


function manualCardValidation(idNumber, recordId, objectType, idTypeName)
{
    $("#msg1").text("ID Type: " + idTypeName);
    $("#msg2").text("ID Number: " + idNumber);
    $("#recordId").val(recordId);
    $("#objectType").val(objectType);
    $("#manualCardValidationModal").modal();


}

//$('#manualCardValidationModal').on('hide.bs.modal', function (e) {
//    e.preventDefault();
//    e.stopPropagation();
//    return false;
//});