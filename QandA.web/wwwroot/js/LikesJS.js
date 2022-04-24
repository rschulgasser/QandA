$(() => {

    const questionId =  $("#question-id").val();
    setInterval(UpdateLikes(questionId), 1000);




    $("#add-like").on('click', function () {


        $('#add-like').addClass('text-danger');
        $('#add-like').prop('disabled', true);

        $("#add-like").attr('class', "oi oi-heart text-danger");

        const questionId = $("#question-id").val();
     
        const liked = true;

        $.post('/home/addlike', { questionId, liked }, function () {

          UpdateLikes(questionId);
        
           
        });
      





    });
    function UpdateLikes(id) {

        $.get('/home/getlikes', { id }, function (numOfLikes) {
         
            $("#likes-count").text(`${numOfLikes}`);

           
        });
    }
    
});