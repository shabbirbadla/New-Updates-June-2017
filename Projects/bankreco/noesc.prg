if messagebox('Do you want to Save the changes ?',4+32+0)=6
	KEYBOARD '{CTRL+W}'
else
	wait wind [Your last modification will be lost] nowait
	KEYBOARD '{CTRL+Q}'
endif