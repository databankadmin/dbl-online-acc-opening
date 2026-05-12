$(function () {
    var _url = "https://onlineservice.databankgroup.com/dblonline/DropZoneUploader";
    var appForm = $("#signup-form");
    var passportPhotosList ="";
    var residenceList = "";
    var businessDocsList ="";



   
    Dropzone.options.firstApplicantIdPhoto = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#firstApplicantIdPhotohidden").val(file.name);
                $("#firstApplicantIdPhoto").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#firstApplicantIdPhotohidden").val('');
                    removeFileFromBackend("_firstApplicantIdPhoto");

                }
            });

            this.on("success", function (file, fileName) {
                $("#_firstApplicantIdPhoto").val(fileName);
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
        }


    };


    //

    Dropzone.options.jointApplicantIdPhoto = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",

        init: function () {
            this.on("addedfile", function (file) {
                $("#jointApplicantIdPhotohidden").val(file.name);
                $("#jointApplicantIdPhoto").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#jointApplicantIdPhotohidden").val('');
                    removeFileFromBackend("_jointApplicantIdPhoto");

                }
            });

            this.on("success", function (file, fileName) {
                $("#_jointApplicantIdPhoto").val(fileName);
            });

            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
        }



    };



 

    Dropzone.options.itfApplicantIdPhoto = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",

        init: function () {
            this.on("addedfile", function (file) {
                $("#itfApplicantIdPhotohidden").val(file.name);
                $("#itfApplicantIdPhoto").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#itfApplicantIdPhotohidden").val('');
                    removeFileFromBackend("_itfApplicantIdPhoto");
                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_itfApplicantIdPhoto").val(fileName);
            });


        }


    };

    
    Dropzone.options.firstJointItfAuthorisedPerson = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#firstJointItfAuthorisedPersonhidden").val(file.name);
                $("#firstJointItfAuthorisedPerson").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#firstJointItfAuthorisedPersonhidden").val('');
                    removeFileFromBackend("_firstJointItfAuthorisedPerson");
                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_firstJointItfAuthorisedPerson").val(fileName);
            });

        }

    };





    
    Dropzone.options.instAuthorisedOfficer1PhotoId = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",

        init: function () {
            this.on("addedfile", function (file) {
                $("#instAuthorisedOfficer1PhotoIdhidden").val(file.name);
                $("#instAuthorisedOfficer1PhotoId").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instAuthorisedOfficer1PhotoIdhidden").val('');
                    removeFileFromBackend("_instAuthorisedOfficer1PhotoId");

                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_instAuthorisedOfficer1PhotoId").val(fileName);
            });
        }

    };



    
    Dropzone.options.instAuthorisedOfficer2PhotoId = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple:false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#instAuthorisedOfficer2PhotoIdhidden").val(file.name);
                $("#instAuthorisedOfficer2PhotoId").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instAuthorisedOfficer2PhotoIdhidden").val('');
                    removeFileFromBackend("_instAuthorisedOfficer2PhotoId");

                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_instAuthorisedOfficer2PhotoId").val(fileName);
            });
        }
    };



    Dropzone.options.instSignatory1 = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#instSignatory1hidden").val(file.name);
                $("#instSignatory1").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instSignatory1hidden").val('');
                    removeFileFromBackend("_instSignatory1");

                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_instSignatory1").val(fileName);
            });

           
        }


    };



    Dropzone.options.instSignatory2 = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#instSignatory2hidden").val(file.name);
                $("#instSignatory2").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instSignatory2hidden").val('');
                    removeFileFromBackend("_instSignatory2");


                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_instSignatory2").val(fileName);
            });

        }

    };

    Dropzone.options.instSignatory3 = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#instSignatory3hidden").val(file.name);
                $("#instSignatory3").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instSignatory3hidden").val('');
                    removeFileFromBackend("_instSignatory3");
                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_instSignatory3").val(fileName);
            });

        }

    };

    Dropzone.options.instSignatory4 = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#instSignatory4hidden").val(file.name);
                $("#instSignatory4").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#instSignatory4hidden").val('');
                    removeFileFromBackend("_instSignatory4");
                }
            });

            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });

            this.on("success", function (file, fileName) {
                $("#_instSignatory4").val(fileName);
            });
        }

    };




    Dropzone.options.proofOfResidence = {
        url: _url + "/Multi",
        paramName: "_files",
        maxFilesize: 200,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 10,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#proofOfResidencehidden").val(file.name);
                $("#proofOfResidence").css("border", "2px dashed #0087f7");
                $(".dropzone.dz-started .dz-message").css("display", "block");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                removeFromList("residence", file.name);
                if (this.files.length === 0) {
                    $("#proofOfResidencehidden").val('');

                }
            });

            this.on("successmultiple", function (file, res) {
                residenceList = residenceList +"," + res;
                $("#residenceList").val(residenceList);
            });

            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
        }

    };

    Dropzone.options.otherBusinessFiles = {
        url: _url + "/Multi",
        paramName: "_files",
        maxFilesize: 200,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 10,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#otherBusinessFileshidden").val(file.name);
                $("#otherBusinessFiles").css("border", "2px dashed #0087f7");
                $(".dropzone.dz-started .dz-message").css("display", "block");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                removeFromList("busdocs", file.name);
                if (this.files.length === 0) {
                    $("#otherBusinessFileshidden").val('');

                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("successmultiple", function (file, res) {
                businessDocsList = businessDocsList+"," + res;
                $("#businessDocsList").val(businessDocsList);

            });

        }


    };





    Dropzone.options.csdCompletedForm = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 200,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".pdf",
        init: function () {
            this.on("addedfile", function (file) {
                //var fileName = file.name.toLowerCase();
                //var ends = fileName.endsWith(".pdf");
                //if (ends === false) {
                //    alert('invalid file type. Uploaded file must be pdf');
                //    this.removeFile(file);
                //    return;
                //}
                $("#csdCompletedFormhidden").val(file.name);
                $("#csdCompletedForm").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#csdCompletedFormhidden").val('');
                    removeFileFromBackend("_csdCompletedForm");

                }
            });

            this.on("success", function (file, fileName) {
                $("#_csdCompletedForm").val(fileName);
            });
        }


    };





    //passportPhotoshidden
    Dropzone.options.passportPhotos = {
        url: _url + "/Multi",
        paramName: "_files",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 10,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#passportPhotoshidden").val(file.name);
                $("#passportPhotos").css("border", "2px dashed #0087f7");
                $(".dropzone.dz-started .dz-message").css("display","block");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                removeFromList("passport", file.name);
                if (this.files.length === 0) {
                    $("#passportPhotoshidden").val('');

                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("successmultiple", function (file,res) {
                passportPhotosList = passportPhotosList+"," + res;
                $("#passportPhotosList").val(passportPhotosList);
            });

        }

    };





    Dropzone.options.firstApplicantSignature = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#firstApplicantSignaturehidden").val(file.name);
                $("#firstApplicantSignature").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#firstApplicantSignaturehidden").val('');
                    removeFileFromBackend("_firstApplicantSignature");
                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_firstApplicantSignature").val(fileName);
            });
        }

    };




    Dropzone.options.jointApplicantSignature = {
        url: _url + "/Single",
        paramName: "_file",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: true,
        uploadMultiple: false,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#jointApplicantSignaturehidden").val(file.name);
                $("#jointApplicantSignature").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#jointApplicantSignaturehidden").val('');
                    removeFileFromBackend("_jointApplicantSignature");
                }
            });
            this.on("error", function (file, message) {
                if (!file.accepted) {
                    alert('File upload error. \n' + message);
                    this.removeFile(file);
                }
            });
            this.on("success", function (file, fileName) {
                $("#_jointApplicantSignature").val(fileName);
            });
        }

    };


    function removeFromList(type, fileName) {
        if (type === "passport") {
            passportPhotosList = passportPhotosList.replace(fileName, "");
            $("#passportPhotosList").val(passportPhotosList);

        }

        else if (type === "busdocs") {
            businessDocsList = businessDocsList.replace(fileName, "");
            $("#businessDocsList").val(businessDocsList);

        }
        else if (type === "residence") {
            residenceList = residenceList.replace(fileName, "");
            $("#residenceList").val(residenceList);

        }
    }

 });









function removeFileFromBackend(ctrl)
{
    $("#" + ctrl).val("");
}

   