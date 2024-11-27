document.getElementById('imageUpload').addEventListener('change', function(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            document.getElementById('preview').src = e.target.result;

            // Example: This should be replaced by actual image processing logic.
            document.getElementById('descriptionText').innerText = "This is a sample description of the uploaded image.";
        };
        reader.readAsDataURL(file);
    } else {
        document.getElementById('preview').src = '';
        document.getElementById('descriptionText').innerText = "Upload an image to see its description.";
    }
});
