$(function () {
    var _url = "/NewAccount";
    var appForm = $("#signup-form");

   
    Dropzone.options.firstApplicantIdPhoto = {
        url: _url + "/FirstApplicantIdPhoto",
        paramName: "firstApplicantIdPhoto",
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
                    removeFileFromBackend("firstApplicantIdPhoto", file.name);

                }
            });
           
        }


    };


    //

    Dropzone.options.jointApplicantIdPhoto = {
        url: _url + "/JointApplicantIdPhoto",
        paramName: "jointApplicantIdPhoto",
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
                    removeFileFromBackend("jointApplicantIdPhoto", file.name);

                }
            });

           
        }



    };



 

    Dropzone.options.itfApplicantIdPhoto = {
        url: _url + "/ItfApplicantIdPhoto",
        paramName: "itfApplicantIdPhoto",
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
                    removeFileFromBackend("itfApplicantIdPhoto", file.name);


                }
            });
            
        }


    };

    
    Dropzone.options.firstJointItfAuthorisedPerson = {
        url: _url + "/FirstJointItfAuthorisedPerson",
        paramName: "firstJointItfAuthorisedPerson",
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
                    removeFileFromBackend("firstJointItfAuthorisedPerson", file.name);


                }
            });
        }

    };





    
    Dropzone.options.instAuthorisedOfficer1PhotoId = {
        url: _url + "/InstAuthorisedOfficer1PhotoId",
        paramName: "instAuthorisedOfficer1PhotoId",
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
                    removeFileFromBackend("instAuthorisedOfficer1PhotoId", file.name);

                }
            });
        }

    };



    
    Dropzone.options.instAuthorisedOfficer2PhotoId = {
        url: _url + "/InstAuthorisedOfficer2PhotoId",
        paramName: "instAuthorisedOfficer2PhotoId",
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
                    removeFileFromBackend("instAuthorisedOfficer2PhotoId", file.name);

                }
            });
        }
    };



    Dropzone.options.instSignatory1 = {
        url: _url + "/InstSignatory1",
        paramName: "instSignatory1",
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
                    removeFileFromBackend("instSignatory1", file.name);

                }
            });


           
        }


    };



    Dropzone.options.instSignatory2 = {
        url: _url + "/InstSignatory2",
        paramName: "instSignatory2",
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
                    removeFileFromBackend("instSignatory2", file.name);


                }
            });

           
        }

    };

    Dropzone.options.instSignatory3 = {
        url: _url + "/InstSignatory3",
        paramName: "instSignatory3",
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
                    removeFileFromBackend("instSignatory3", file.name);


                }
            });
        }

    };

    Dropzone.options.instSignatory4 = {
        url: _url + "/InstSignatory4",
        paramName: "instSignatory4",
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
                    removeFileFromBackend("instSignatory4", file.name);


                }
            });

           
        }

    };




    Dropzone.options.proofOfResidence = {
        url: _url + "/ProofOfResidence",
        paramName: "proofOfResidenceFiles",
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
                removeFileFromBackend("proofOfResidenceFiles", file.name);
                if (this.files.length === 0) {
                    $("#proofOfResidencehidden").val('');

                }
            });

        }

    };

    Dropzone.options.otherBusinessFiles = {
        url: _url + "/OtherBusinessFiles",
        paramName: "otherBusinessFiles",
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
                removeFileFromBackend("otherBusinessFiles", file.name);
                if (this.files.length === 0) {
                    $("#otherBusinessFileshidden").val('');

                }
            });

           
        }


    };





    Dropzone.options.csdCompletedForm = {
        url:_url + "/CsdCompletedForm",
        paramName: "csdCompletedForm",
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
                var fileName = file.name.toLowerCase();
                var ends = fileName.endsWith(".pdf");
                if (ends === false) {
                    alert('invalid file type. Uploaded file must be pdf');
                    this.removeFile(file);
                    return;
                }
                $("#csdCompletedFormhidden").val(file.name);
                $("#csdCompletedForm").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#csdCompletedFormhidden").val('');
                    removeFileFromBackend("csdCompletedForm", file.name);

                }
            });
            
        }


    };





    //passportPhotoshidden
    Dropzone.options.passportPhotos = {
        url: _url + "/PassportPhotos",
        paramName: "passportPhotos",
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
                removeFileFromBackend("passportPhotos", file.name);
                if (this.files.length === 0) {
                    $("#passportPhotoshidden").val('');

                }
            });

          
        }

    };





    Dropzone.options.firstApplicantSignature = {
        url: _url + "/FirstApplicantSignature",
        paramName: "firstApplicantSignature",
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
                    removeFileFromBackend("firstApplicantSignature", file.name);
                }
            });


        }

    };




    Dropzone.options.jointApplicantSignature = {
        url: _url + "/JointApplicantSignature",
        paramName: "jointApplicantSignature",
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
                    removeFileFromBackend("jointApplicantSignature", file.name);
                }
            });


        }

    };



 });













function removeFileFromBackend(type, fileName)
{
    $.ajax({
        url: '/NewAccount/DropFile?type=' + type + "&fileName=" + fileName,
        type: "GET",
        async: true,
        processData: false,
        cache: false,
        success: function (response) {
             

        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });


}


   