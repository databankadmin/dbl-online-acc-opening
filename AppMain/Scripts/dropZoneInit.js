$(function () {
  var _url = "/NewAccount/Initiate";

 Dropzone.options.csdImageDiv = {
            url: _url,
            paramName: "csdIdImg",
            maxFilesize: 20,//filesize in MB
            autoProcessQueue: false,
            uploadMultiple: true,
            parallelUploads: 100,
            maxFiles: 1,//maximum number of files 
            addRemoveLinks: true,
            dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
          acceptedFiles: ".jpeg,.jpg,.png,.gif",

     init: function () {
         this.on("addedfile", function (file)
         {
             $("#csdimagedivhidden").val(file.name);
             $("#csd-image-div").css("border", "2px dashed #0087f7");
         });

         this.on("maxfilesexceeded", function (file)
         {
             alert("File limit exceeded");
             this.removeFile(file);
         });
         this.on("removedfile", function (file) {
             if (this.files.length === 0) {
                 $("#csdimagedivhidden").val('');

             }
         });
     }
    };


   
    Dropzone.options.firstApplicantIdPhoto = {
        url: _url,
        paramName: "firstApplicantIdPhoto",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }


    };


    //

    Dropzone.options.jointApplicantIdPhoto = {
        url: _url,
        paramName: "jointApplicantIdPhoto",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",

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

                }
            });
        }



    };



 

    Dropzone.options.itfApplicantIdPhoto = {
        url: _url,
        paramName: "itfApplicantIdPhoto",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",

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

                }
            });
        }


    };

    
    Dropzone.options.firstJointItfAuthorisedPerson = {
        url: _url,
        paramName: "firstJointItfAuthorisedPerson",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }

    };





    
    Dropzone.options.instAuthorisedOfficer1PhotoId = {
        url: _url,
        paramName: "instAuthorisedOfficer1PhotoId",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",

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

                }
            });
        }

    };



    
    Dropzone.options.instAuthorisedOfficer2PhotoId = {
        url: _url,
        paramName: "instAuthorisedOfficer2PhotoId",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }

    };



    Dropzone.options.instSignatory1 = {
        url: _url,
        paramName: "instSignatory1",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }


    };



    Dropzone.options.instSignatory2 = {
        url: _url,
        paramName: "instSignatory2",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }

    };

    Dropzone.options.instSignatory3 = {
        url: _url,
        paramName: "instSignatory3",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }

    };

    Dropzone.options.instSignatory4 = {
        url: _url,
        paramName: "instSignatory4",
        maxFilesize: 20,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 1,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
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

                }
            });
        }

    };




    Dropzone.options.proofOfResidence = {
        url: _url,
        paramName: "proofOfResidenceFiles",
        maxFilesize: 200,//filesize in MB
        autoProcessQueue: false,
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
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#proofOfResidencehidden").val('');

                }
            });
        }

    };

    Dropzone.options.otherBusinessFiles = {
        url: _url,
        paramName: "otherBusinessFiles",
        maxFilesize: 200,//filesize in MB
        autoProcessQueue: false,
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 10,//maximum number of files 
        addRemoveLinks: true,
        dictDefaultMessage: "<i class='fa fa-plus fa-2x'></i>",
        acceptedFiles: ".pdf",
        init: function () {
            this.on("addedfile", function (file) {
                $("#otherBusinessFileshidden").val(file.name);
                $("#otherBusinessFiles").css("border", "2px dashed #0087f7");
            });

            this.on("maxfilesexceeded", function (file) {
                alert("File limit exceeded");
                this.removeFile(file);
            });
            this.on("removedfile", function (file) {
                if (this.files.length === 0) {
                    $("#otherBusinessFileshidden").val('');

                }
            });
        }


    };
    
        });




   