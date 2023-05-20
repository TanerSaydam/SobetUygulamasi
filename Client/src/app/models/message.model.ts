import { UserModel } from "./user.model";

export class MessageModel{
    id: number = 0;
    userId: number = 0;
    user: UserModel = new UserModel();
    chatId: number = 0;
    timestamp: string = "";
    text: string = "";
}