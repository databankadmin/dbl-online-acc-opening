function markBorderRed(itemId,msg)
{
    $("#" + itemId).css("border", "2px dashed red");
    alert(msg);
    $("#" + itemId).focus();

}
(function ($) {
    


    var form = $("#signup-form");
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.before(error);
        },
        rules: {
    //        depositorParticipant: {
    //           // email: true,
				//required:true
    //        },
    //        csdTitle: {
    //            required: true,
    //            min:1
    //        },

    //        csdSurname: {
    //            required: true
    //        },
    //        csdResidentialStatus: {
    //            required: true
    //        },

    //        csdHomeTel: {
    //            required: true
    //        },
    //        csdEmail: {
    //            email: true,
    //            required: true
    //        },
    //        csdDOB: {
    //            required: true,
    //            date:true
    //        },

    //        csdNationality: {
    //            required: true
    //        },
    //        csdIdType: {
    //            required: true
    //        },
    //        csdIdNumber: {
    //            required: true
    //        },
    //        csdIDIssuePlace: {
    //            required: true
    //        },
    //        csdBoughtTBill: {
    //            required:true
    //        },
    //        existingCSDClientID: {
    //            minlength:13
    //        },
    //        existingCSDClientIDArr2: {
    //            minlength: 2
    //        },
    //        existingCSDClientIDArr3: {
    //            minlength: 2
    //        },

    //        csdBankId: {
    //            required: true
    //        },
    //        csdBankBranch: {
    //            required: true
    //        },
    //        csdBankAccn: {
    //            required: true,
    //            number:true
    //        },
    //        csdDeclareFullname: {
    //            required:true
    //        },

    //        clientCSDSecAccNo: {
    //            minlength: 4
    //        },
    //        clientCSDSecAccNo2: {
    //            minlength: 1
    //        },
    //        clientCSDSecAccNo3: {
    //            minlength: 13
    //        },
    //        existingCSDClientIDArr4: {
    //            minlength: 2
    //        },
    //        existingCSDClientIDArr5: {
    //            minlength: 2
    //        }






        },

		 messages : {
            //email: {
            //    email: 'Not a valid email address <i class="zmdi zmdi-info"></i>'
            //},
            // fname:{
            //     fname: 'fname required <i class="zmdi zmdi-info"></i>'
            // }
             //csdTitle: 'select title',
             //csdResidentialStatus: 'select residential status',
             //csdEmail: 'provide a valid email',
             //csdNationality: 'select nationality',
             //csdIdType: 'select card type',
             //csdBoughtTBill: 'select item',
             //csdBankId: 'select bank',
             //csdBankAccn:'enter a valid account number'



        },
		
        onfocusout: function(element) {
            $(element).valid();
        },
    });
    form.children("div").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "fade",
        stepsOrientation: "vertical",
        titleTemplate: '<div class="title"><span class="step-number">#index#</span><span class="step-text">#title#</span></div>',
        labels: {
            previous: 'Previous',
            next: 'Next',
            finish: 'Submit Application',
            current: ''
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            const urlParams = new URLSearchParams(window.location.search);
            var accType = atob(urlParams.get('acc_type'));
            var accountTypeId = Number(accType);

           

            if (currentIndex === 0) {//DBL  form; individual, joint, itf or institutional
               
                if (accountTypeId === 1) {//single
                    if ($("#firstApplicantIdPhotohidden").val() === '')
                    {
                        markBorderRed('firstApplicantIdPhoto', 'upload photo ID');
                        return;
                    }

                    if ($("#firstApplicantSignaturehidden").val() === ''  &&  $("#firstApplicantSignatureCapture").attr('src')==='' ) {
                        markBorderRed('firstApplicantSignature', 'upload first applicant signature');
                        return;
                    }

                    if ($("#instructionsEmploymentDetailsSourceOfIncomeIds").val() === null) {
                        markBorderRed('instructionsEmploymentDetailsSourceOfIncomeIdsDiv', 'indicate source of funds');
                        return;
                    }
                }




                if (accountTypeId === 2) {//joint

                    if ($("#firstApplicantIdPhotohidden").val() === '') {
                        markBorderRed('firstApplicantIdPhoto', 'upload first applicant photo ID');
                        return;
                    }

                    if ($("#firstApplicantSignaturehidden").val() === '' && $("#firstApplicantSignatureCapture").attr('src') === '') {
                        markBorderRed('firstApplicantSignature', 'upload first applicant signature');
                        return;
                    }



                    if ($("#jointApplicantIdPhotohidden").val() === '') {
                        markBorderRed('jointApplicantIdPhoto', 'upload joint photo ID');
                        return;
                    }

                    if ($("#jointApplicantSignaturehidden").val() === '' && $("#jointApplicantSignatureCapture").attr('src') === '') {
                        markBorderRed('jointApplicantSignature', 'upload joint applicant signature');
                        return;
                    }

                    if ($("#instructionsEmploymentDetailsSourceOfIncomeIds").val() === null) {
                        markBorderRed('instructionsEmploymentDetailsSourceOfIncomeIdsDiv', 'indicate source of funds');
                        return;
                    }
                }



                if (accountTypeId === 3) {//itf

                    if ($("#firstApplicantIdPhotohidden").val() === '') {
                        markBorderRed('firstApplicantIdPhoto', 'upload photo ID');
                        return;
                    }

                    if ($("#firstApplicantSignaturehidden").val() === '' && $("#firstApplicantSignatureCapture").attr('src') === '') {
                        markBorderRed('firstApplicantSignature', 'upload first applicant signature');
                        return;
                    }
                    if ($("#itfApplicantIdPhotohidden").val() === '') {
                        markBorderRed('itfApplicantIdPhoto', 'upload ITF photo ID');
                        return;
                    }

                    if ($("#instructionsEmploymentDetailsSourceOfIncomeIds").val() === null) {
                        markBorderRed('instructionsEmploymentDetailsSourceOfIncomeIdsDiv', 'indicate source of funds');
                        return;
                    }
                }
                if (accountTypeId === 4) {//inst

                    if ($("#instAuthorisedOfficer1PhotoIdhidden").val() === '') {
                        markBorderRed('instAuthorisedOfficer1PhotoId', 'upload photo ID');
                        return;
                    }
                    if ($("#instAuthorisedOfficer2PhotoIdhidden").val() === '') {
                        markBorderRed('instAuthorisedOfficer2PhotoId', 'upload photo ID');
                        return;
                    }




                    if ($("#instSignatory1hidden").val() === '') {
                        markBorderRed('instSignatory1', 'upload signature 1' && $("#instSign1Capture").attr('src') === '');
                        return;
                    }

                    if ($("#instSignatory2hidden").val() === '') {
                        markBorderRed('instSignatory2', 'upload signature 2' && $("#instSign2Capture").attr('src') === '');
                        return;
                    }


                    if ($("#intSourceOfIncomeIds").val() === null) {
                        markBorderRed('intSourceOfIncomeDiv', 'indicate source of funds');
                        return;
                    }
                }


                if (accountTypeId === 1 || accountTypeId === 2 || accountTypeId === 3)
                {
                    //if ($("#firstJointItfAuthorisedPersonhidden").val() === '') {
                    //    markBorderRed('firstJointItfAuthorisedPerson', 'upload authorised person photo ID');
                    //    return;
                    //}
                    
                }

                form.parent().parent().parent().find('.footer').removeClass('footer-0').addClass('footer-' + currentIndex + '');
            }




            if (currentIndex === 1) {//file uploads
                if ($("#proofOfResidencehidden").val() === '') {
                    markBorderRed('proofOfResidence', 'upload proof of residence');
                    return;
                }

                if ($("#passportPhotoshidden").val() === '') {
                    markBorderRed('passportPhotos', 'upload passport photo');
                    return;
                }


                if (accountTypeId===4) {//inst
                    if ($("#otherBusinessFileshidden").val() === '') {
                        markBorderRed('otherBusinessFiles', 'upload other business documents');
                        return;
                    }
                }
                form.parent().parent().parent().find('.footer').removeClass('footer-1').addClass('footer-' + currentIndex + '');
            }




            if (currentIndex === 2) {
                form.parent().parent().parent().find('.footer').removeClass('footer-2').addClass('footer-' + currentIndex + '');
            }

            if (currentIndex === 3) {//aml
                form.parent().parent().parent().find('.footer').removeClass('footer-3').addClass('footer-' + currentIndex + '');
            }


            if (currentIndex === 4) {//csd form
                if ($("#csdCompletedFormhidden").val() === '') {
                    markBorderRed('csdCompletedForm', 'upload your completed CSD form');
                    return;
                }
                form.parent().parent().parent().append('<div class="footer footer-' + currentIndex + '"></div>');
            }





            if (currentIndex === 5) {//indemnity
                form.parent().parent().parent().find('.footer').removeClass('footer-4').addClass('footer-' + currentIndex + '');
            }




            form.validate().settings.ignore = ":disabled,:hidden";
            return form.valid();
        },
        onFinishing: function(event, currentIndex) {
            form.validate().settings.ignore = ":disabled";
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            form.submit();
           // alert('Submited');
        },
        onStepChanged: function(event, currentIndex, priorIndex) {

            return true;
        }
    });

    jQuery.extend(jQuery.validator.messages, {
        required: "",
        remote: "",
        email: "",
        url: "",
        date: "",
        dateISO: "",
        number: "",
        digits: "",
        creditcard: "",
        equalTo: "",
        fname:""
    });

    $.dobPicker({
        daySelector: '#birth_date',
        monthSelector: '#birth_month',
        yearSelector: '#birth_year',
        dayDefault: '',
        monthDefault: '',
        yearDefault: '',
        minimumAge: 0,
        maximumAge: 120
    });
    var marginSlider = document.getElementById('slider-margin');
    if (marginSlider != undefined) {
        noUiSlider.create(marginSlider, {
              start: [1100],
              step: 100,
              connect: [true, false],
              tooltips: [true],
              range: {
                  'min': 100,
                  'max': 2000
              },
              pips: {
                    mode: 'values',
                    values: [100, 2000],
                    density: 4
                    },
                format: wNumb({
                    decimals: 0,
                    thousand: '',
                    prefix: '$ ',
                })
        });
        var marginMin = document.getElementById('value-lower'),
	    marginMax = document.getElementById('value-upper');

        marginSlider.noUiSlider.on('update', function ( values, handle ) {
            if ( handle ) {
                marginMax.innerHTML = values[handle];
            } else {
                marginMin.innerHTML = values[handle];
            }
        });
    }
})(jQuery);