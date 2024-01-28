import { Component } from '@angular/core';
import { tap } from 'rxjs';
import { MessageHistoryResult } from 'src/app/shared/models/message-hisotry';
import { MessageResult } from 'src/app/shared/models/message-result';
import { StudentProfile } from 'src/app/shared/models/student-profile';
import { UserResult } from 'src/app/shared/models/user-result';
import { ChatRequestsService } from 'src/app/shared/services/chat.requests.service';
import { ChatService } from 'src/app/shared/services/chat.service';

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
  searchText: string = '';
  contactsData: UserResult[] = [];
  message: string = '';
  chatsHistory: MessageHistoryResult[] = [];
selected: any;
color: any;
  constructor(
    // private teachersService: TeachersService,
    public chatService: ChatService,

    public chatRequestsService: ChatRequestsService,
      ) 
      
  {}
  selectedChatIndex: number | null = null;

  ngOnInit(): void {
    this.chatRequestsService
      .getContacts()
      .pipe(tap((contacts) => (this.contactsData = contacts)))
      .subscribe();

      this.chatService.startConnection().then(() => {
        this.chatService.registerOnMessageReceived((senderUserId, message) => {
          // Handle the received message here (update your chat UI)
          const messageObject:Message ={
            text:message,
            sentBy:'friend'
          }
          this.getMessageHistory();
          this.selectedChat.messages.push(messageObject)
          console.log(`Message from ${senderUserId}: ${message}`);
    })
    
    
  })
  
  this.chatRequestsService.getMessageHistory() 
  .pipe(tap((chat) => (this.chatsHistory = chat)))
  .subscribe();

  
  
}

  getMessageHistory() {
    this.chatRequestsService
      .getMessageHistory()
      .pipe(tap((chat) => (this.chatsHistory = chat)))
      .subscribe();
  }

    filteredChatHistory(): MessageHistoryResult[] {
      return this.chatsHistory.filter(chat =>
        chat.firstName.toLowerCase().includes(this.searchText.toLowerCase())
      );
    }
    
  selectedChat: ChatEntry = {
    personName: '',
    messages: [] as Message[],
    receiverUserId: '',
  };

  newMessage: string = '';
  selectChat(personName: string, userId: string , index : number): void {
    this.selectedChat = {
      personName: personName,
      messages: [],
      receiverUserId: userId,
    };
    this.selectedChatIndex = index;    
   

    this.chatRequestsService.getMessagesInChat(userId).subscribe(
      (messageResults: MessageResult[]) => {
        this.selectedChat.messages = messageResults.map((msgResult) => {
          return {
            text: msgResult.content,
            sentBy: msgResult.senderId === userId ? 'friend' : 'me',
          };
        });
      },
      (error) => {
        console.error('Error loading messages:', error);
      }
    );
  }

  sendMessage(): void {
    if (!this.newMessage.trim()) return;

    this.chatService
      .sendMessageToUser(this.selectedChat.receiverUserId, this.newMessage)
      .then(() => {
        const newMessage: Message = {
          text: this.newMessage,
          sentBy: 'me',
        };
        this.selectedChat.messages.push(newMessage);

        this.newMessage = '';

        console.log('Message sent');
      })
      .catch((error) => {
        console.error('Error sending message:', error);
      });
    this.getMessageHistory();
  }
}
