
function DateChanged(dob, ctrl) {
    //jointApplicantDOB
    minAge = 18;
    age = computeAge(new Date(dob));
    if (Number(age) < 0) {
        age = 0;
    }
    if (ctrl === "firstApplicantDOB" || "jointApplicantDOB") {
        if (age < minAge) {
            swal("Applicant MUST NOT be less than " + minAge + " years.\nAge is: " + age);
            $("#" + ctrl).val("");
        }
    }

}





function computeAge(birth) {
    var today = new Date();
    var nowyear = today.getFullYear();
    var nowmonth = today.getMonth();
    var nowday = today.getDate();

    var birthyear = birth.getFullYear();
    var birthmonth = birth.getMonth();
    var birthday = birth.getDate();

    var age = nowyear - birthyear;
    var age_month = nowmonth - birthmonth;
    var age_day = nowday - birthday;

    if (age_month < 0 || (age_month === 0 && age_day < 0)) {
        age = parseInt(age) - 1;
    }
    return parseInt(age);


}



function ValidateUpload(inputId) {
  //  alert("called...");
    
    var file_size = $('#' + inputId)[0].files[0].size;
    var file_type = $('#' + inputId)[0].files[0].type;
    var is_file_ok = false;

    switch (file_type) {

        case 'application/pdf':
        case 'image/png':
        case 'image/gif':
        case 'image/jpeg':
        case 'image/pjpeg':
        case 'image/x-png':
      //  case 'application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
      //  case 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
      //  case 'application/vnd.ms-excel':



        
            is_file_ok = true;
            break;
        default:
            is_file_ok = false;
    }


    if (is_file_ok == false) {
 bootbox.alert("<b>File Upload Error!</b> <br/>Selected file is Invalid. <br/>");
        $('#' + inputId).val("");
    }

   

    return true;
}



function ValidatePdfFile(inputId) {
    var file_size = $('#' + inputId)[0].files[0].size;
    var file_type = $('#' + inputId)[0].files[0].type;
    var is_file_ok = false;

    switch (file_type) {

    case 'application/pdf':
 
        is_file_ok = true;
        break;
    default:
        is_file_ok = false;
    }


    if (is_file_ok == false) {
        bootbox.alert("<b>File Upload Error!</b> <br/>Selected file is Invalid. <br/>");
        $('#' + inputId).val("");
    }



    return true;
}





function validateExcel(inputId) {

    var file_size = $('#' + inputId)[0].files[0].size;
    var file_type = $('#' + inputId)[0].files[0].type;
    var file_name = $('#' + inputId)[0].files[0].name;
    var is_file_ok = false;



    switch (file_type) {

        case 'application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
        case 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
        case 'application/vnd.ms-excel':



            is_file_ok = true;
            break;
        default:
            is_file_ok = false;
    }


    if (is_file_ok == false) {
        bootbox.alert("<b style='color:black'>File Upload Error!</b> <br/><span style='color:black'>Selected file is Invalid.</span> <br/><span style='color:black'>Only Excel files are accepted</span>");
        $('#' + inputId).val("");
    }



    return true;
}







function validateUploadImageOnly(inputId) {
    var file_size = $('#' + inputId)[0].files[0].size;
    var file_type = $('#' + inputId)[0].files[0].type;
    var is_file_ok = false;
    switch (file_type) {
        case 'image/png':
        case 'image/gif':
        case 'image/jpeg':
        case 'image/pjpeg':
        case 'image/x-png':
            

            is_file_ok = true;
            break;
        default:
            is_file_ok = false;
    }

    if (is_file_ok == false) {
        alert("File Upload Error!\nSelected file is Invalid. Only image files are accepted");
        $('#' + inputId).val("");
    }

    else if (file_size > 5242880) {
        alert("File Upload Error! File size is greater than 5MB");
        $('#' + inputId).val("");
    }


    return true;
}


