import { Component } from '@angular/core';
import { tap } from 'rxjs';
import { StudentProfile } from 'src/app/shared/models/student-profile';
import { UserResult } from 'src/app/shared/models/user-result';
import { ChatRequestsService } from 'src/app/shared/services/chat.requests.service';
import { ChatService } from 'src/app/shared/services/chat.service';
import { TeachersService } from 'src/app/shared/services/teachers.service';

interface ChatEntry {
  personName: string;
  messages: Message[];
  receiverUserId: string;
}

interface Message {
  text: string;
  sentBy: 'me' | 'friend';
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent {
  
  contactsData: UserResult[] = [];
  message: string = "";

  constructor(
    // private teachersService: TeachersService,
    public chatService: ChatService,
    public chatRequestsService: ChatRequestsService
      ) 
  {}


  ngOnInit(): void {
    this.chatRequestsService
      .getContacts()
      .pipe(tap((contacts) => (this.contactsData = contacts)))
      .subscribe();

      this.chatService.startConnection().then(() => {
        this.chatService.registerOnMessageReceived((senderUserId, message) => {
          // Handle the received message here (update your chat UI)
          console.log(`Message from ${senderUserId}: ${message}`);
    })
  })
}

  // people = [
  //   { name: 'Alice', avatar: 'path/to/avatar1.jpg' },
  //   { name: 'Bob', avatar: 'path/to/avatar2.jpg' },
  //   { name: 'Charlie', avatar: 'path/to/avatar3.jpg' },
  //   { name: 'David', avatar: 'path/to/avatar4.jpg' },
  //   { name: 'Eva', avatar: 'path/to/avatar5.jpg' },
  //   { name: 'Frank', avatar: 'path/to/avatar6.jpg' },
  //   { name: 'Grace', avatar: 'path/to/avatar7.jpg' },
  //   { name: 'Henry', avatar: 'path/to/avatar8.jpg' },
  //   { name: 'Ivy', avatar: 'path/to/avatar9.jpg' },
  //   { name: 'Jack', avatar: 'path/to/avatar10.jpg' },
  //   { name: 'Katherine', avatar: 'path/to/avatar11.jpg' },
  //   { name: 'Leo', avatar: 'path/to/avatar12.jpg' },
  //   { name: 'Mia', avatar: 'path/to/avatar13.jpg' },
  //   { name: 'Nathan', avatar: 'path/to/avatar14.jpg' },
  //   { name: 'Olivia', avatar: 'path/to/avatar15.jpg' },
  //   { name: 'Peter', avatar: 'path/to/avatar16.jpg' },
  //   { name: 'Quinn', avatar: 'path/to/avatar17.jpg' },
  //   { name: 'Rachel', avatar: 'path/to/avatar18.jpg' },
  //   { name: 'Samuel', avatar: 'path/to/avatar19.jpg' },
  //   { name: 'Tara', avatar: 'path/to/avatar20.jpg' },
  //   { name: 'Ulysses', avatar: 'path/to/avatar21.jpg' },
  //   { name: 'Victoria', avatar: 'path/to/avatar22.jpg' },
  //   { name: 'Walter', avatar: 'path/to/avatar23.jpg' },
  //   { name: 'Xena', avatar: 'path/to/avatar24.jpg' },
  //   { name: 'Yasmine', avatar: 'path/to/avatar25.jpg' },
  //   { name: 'Zane', avatar: 'path/to/avatar26.jpg' },
  //   { name: 'Anna', avatar: 'path/to/avatar27.jpg' },
  //   { name: 'Benjamin', avatar: 'path/to/avatar28.jpg' },
  //   { name: 'Chloe', avatar: 'path/to/avatar29.jpg' },
  //   { name: 'Dylan', avatar: 'path/to/avatar30.jpg' }
  // ];



  selectedChat: ChatEntry = {
    personName: '',
    messages: [] as Message[],
    receiverUserId: "",
  };

  // topRowTranslation = '0'; // Translation for the top row sliding effect
  newMessage: string = '';

  selectChat(personName: string, userId: string): void {
    // Find the chat entry in chatHistory based on the person's name
    this.selectedChat = {
      personName: personName,
      messages: [] as Message[],
      receiverUserId: userId
    };
  }

  // getLastMessage(personName: string): string {
  //   // Find the chat entry in chatHistory based on the person's name
  //   const chatEntry = this.chatHistory.find(entry => entry.personName === personName);

  //   if (chatEntry) {
  //     // Get the last message from the messages array
  //     const lastMessage = chatEntry.messages[chatEntry.messages.length - 1];
  //     return lastMessage ? lastMessage.text : 'No messages';
  //   } else {
  //     return 'No messages';
  //   }
  // }

  // generateFakeMessages(count: number): Message[] {
  //   const messages: Message[] = [];
  //   for (let i = 0; i < count; i++) {
  //     let text = '';
  //     switch (i % 6) {
  //       case 0:
  //         text = 'Hi there!';
  //         break;
  //       case 1:
  //         text = 'Hello!';
  //         break;
  //       case 2:
  //         text = 'How are you?';
  //         break;
  //       case 3:
  //         text = 'Good!, You?';
  //         break;
  //       case 4:
  //         text = 'Me too';
  //         break;
  //       case 5:
  //         text = 'Ok, bye';
  //         break;
  //       default:
  //         text = 'Default message';
  //         break;
  //     }

  //     messages.push({
  //       text: text,
  //       sentBy: i % 2 === 0 ? 'me' : 'friend',
  //     });
  //   }
  //   return messages;
  // }

  sendMessage(): void {
    if (!this.newMessage.trim()) return; // Prevent sending empty messages
  
    this.chatService.sendMessageToUser(this.selectedChat.receiverUserId, this.newMessage).then(() => {
      const newMessage: Message = {
        text: this.newMessage,
        sentBy: 'me',
      };
  
      // Add the new message to the selectedChat messages array
      this.selectedChat.messages.push(newMessage);
  
      // Clear the input after sending the message
      this.newMessage = '';
  
      console.log('Message sent');
    }).catch(error => {
      console.error('Error sending message:', error);
      // Handle error (e.g., show an error message to the user)
    });
  }
}

