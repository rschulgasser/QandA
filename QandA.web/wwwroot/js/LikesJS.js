$(() => {

    console.log("dfdf");
    $("#add-like").on('click', function () {

        $("#likes").append(` <br /> <br /> <span style="font-size: 40px; cursor: pointer;" class="oi oi-heart text-danger">heart</span>`);
        console.log("in");
        const QuestionId = $(this).data('question-id');
        const liked = true;


        $.post('/home/addlike', { QuestionId, liked }, function () {
           

            $("#add-like").remove();

            UpdateLikes(QuestionId);

         
            
          
       

           
        });
      
     

    });
    function UpdateLikes(id) {

        $.get('/home/getlikes', { id }, function (numOfLikes) {
         
            $("#likes-count").text(`${numOfLikes}`);

           
        });
    }
    
});