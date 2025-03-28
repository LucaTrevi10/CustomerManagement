window.BlazorDownloadFile = (fileName, fileBytes) => {
    var link = document.createElement('a');
    link.href = URL.createObjectURL(new Blob([new Uint8Array(fileBytes)], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }));
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};