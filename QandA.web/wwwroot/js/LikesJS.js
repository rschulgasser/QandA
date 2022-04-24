$(() => {

    const questionId =  $("#question-id").val();
    setInterval(UpdateLikes(questionId), 1000);


    console.log(questionId);
    $("#add-like").on('click', function () {

    //  $("#add-like").remove();
      
        console.log("in");
        $('#add-like').addClass('text-danger');
        $('#add-like').prop('disabled', true);

        $("#add-like").attr('class', "oi oi-heart text-danger");

        const questionId = $("#question-id").val();
     
        const liked = true;

        $.post('/home/addlike', { questionId, liked }, function () {

            //$("#add-like").addClass('text-danger');
            //$("#add-like").prop('disabled', true);
          UpdateLikes(questionId);
         //   $("#add-like").attr('class', "oi oi-heart text-danger");
         
       //  $("#question-id").append(` <br /> <br /> <h1> <span style="font-size: 40px; cursor: pointer;" class="oi oi-heart text-danger">heart</span> </h1>`);
          
       

           
        });
      
     

    });
    function UpdateLikes(id) {

        $.get('/home/getlikes', { id }, function (numOfLikes) {
         
            $("#likes-count").text(`${numOfLikes}`);

           
        });
    }
    
});