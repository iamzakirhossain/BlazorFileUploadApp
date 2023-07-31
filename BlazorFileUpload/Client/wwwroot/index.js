
/*window.triggerFileDownload= (fileName, url)=>{*/
window.triggerFileDownload= (fName)=>{
    //        const anchorElement = document.createElement('a');
    //anchorElement.href = url;
    //anchorElement.download = fileName ?? '';
    //anchorElement.click();
    //anchorElement.remove();

    var fileName = "files/" + fName;

    var progress = document.getElementById("progress");
    var progressText = document.getElementById("progress-text");
    var downloadProgressText = document.getElementById("download-progress-text");

    function abort() {
        request.abort();
    }



        var startTime = new Date().getTime();

        request = new XMLHttpRequest();

        request.responseType = 'blob';
        request.open('get', fileName, true);
        request.send();
   

        request.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var obj = window.URL.createObjectURL(this.response);

                document.getElementById('save-file').setAttribute('href', obj);
                document.getElementById('save-file').setAttribute('class', "btn btn-success");
                document.getElementById('save-file').setAttribute('download', fileName);
                document.getElementById('save-file').click();
                document.getElementById("modal-body").setAttribute('style', 'display:none !important;');
                document.getElementById("download-completed").setAttribute("style", "display:block !important;padding-top:50px; padding-bottom:50px;");


                setTimeout(function () {
                    window.URL.revokeObjectURL(obj);
                }, 60 * 1000);
            }
        };

    request.onprogress = function (e) {
        document.getElementById("download-completed").setAttribute("style", "display:none !important;");
            var percent_complete = (e.loaded / e.total) * 100;
            percent_complete = Math.floor(percent_complete);

            var duration = (new Date().getTime() - startTime) / 1000;
            var bps = e.loaded / duration;
            var kbps = bps / 1024;
            kbps = Math.floor(kbps);

            var time = (e.total - e.loaded) / bps;
            var seconds = time % 60;
            var minutes = time / 60;

            seconds = Math.floor(seconds);
            minutes = Math.floor(minutes);

            progress.setAttribute("aria-valuemax", e.total);
            progress.setAttribute("aria-valuenow", e.loaded);
            progress.style.width = percent_complete + "%";
            progress.innerHTML = percent_complete + "%";

            downloadProgressText.innerHTML = kbps + " KB / s" + "<br>" + minutes + " min " + seconds + " sec remaining";


        };

    console.log("Done");

}


