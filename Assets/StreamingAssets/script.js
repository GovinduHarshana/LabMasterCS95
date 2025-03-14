function formatDoc(cmd, value=null){
    if(value){
        document.execCommand(cmd,false,value);
    }else{
        document.execCommand(cmd);
    }
}

function addLink(){
    const url = prompt('Insert Url');
    //document.execCommand('createLink',false,url)
    formatDoc('createLink' , url);
}

const content = document.getElementById('content');

content.addEventListener('mouseenter',function(){
    const a = content.querySelectorAll('a');
    a.forEach(item=>{
        item.addEventListener('mouseenter',function(){
            content.setAttribute('contenteditable', false);
            item.target = '_blank';
        })
        item.addEventListener('mouseleave',function(){
            content.setAttribute('contenteditable', true);

        })
    })
})

function formatDoc(command, value) {
    document.execCommand(command, false, value);
    document.getElementById('text-editor').focus();
  }

  document.getElementById('fontColorButton').addEventListener('click', function() {
    togglePopup('fontColorPopup');
  });

  document.getElementById('highlighterButton').addEventListener('click', function() {
    togglePopup('highlightPopup');
  });

  function togglePopup(popupId) {
    const popup = document.getElementById(popupId);
    popup.style.display = popup.style.display === 'block' ? 'none' : 'block';
  }

  function hidePopup(popupId){
    const popup = document.getElementById(popupId);
    popup.style.display = 'none';
  }

  window.addEventListener('click', function(event) {
    if (!event.target.matches('#fontColorButton') && !event.target.matches('.color-option') && !event.target.matches('#highlighterButton') && !event.target.matches('.highlight-color')) {
      hidePopup('fontColorPopup');
      hidePopup('highlightPopup');
    }
  });
  function formatDoc(command, value) {
    document.execCommand(command, false, value);
    document.getElementById('text-editor').focus();
  }

  document.getElementById('imageUpload').addEventListener('change', function(event) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = function(e) {
        formatDoc('insertImage', e.target.result);
      };
      reader.readAsDataURL(file);
    }
  });

const filename = document.getElementById('filename');

function fileHandle(value) {
	if(value === 'new') {
		content.innerHTML = '';
		filename.value = 'untitled';
	} else if(value === 'txt') {
		const blob = new Blob([content.innerText])
		const url = URL.createObjectURL(blob)
		const link = document.createElement('a');
		link.href = url;
		link.download = `${filename.value}.txt`;
		link.click();
	} else if(value === 'pdf') {
		html2pdf(content).save(filename.value);
	}
}

async function saveNote() {
  const noteText = document.getElementById("noteText").value;

  if (!noteText) {
      alert("Please enter some text before saving.");
      return;
  }

  const response = await fetch("http://localhost:3000/saveNote", {
      method: "POST",
      headers: {
          "Content-Type": "application/json",
      },
      body: JSON.stringify({ note: noteText }),
  });

  const result = await response.json();
  alert(result.message);
}