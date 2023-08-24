type SenderWithUsername = {
  id: string;
  username: string | undefined;
};

type SenderWithoutUsername = {
  id: string;
};

type MessageWithSender = {
  id: string;
  channelId: string;
  sender: Sender;
};

type MessageWithoutSender = {
  id: string;
  channelId: string;
};

export type Sender = SenderWithUsername | SenderWithoutUsername;

export type Message = MessageWithSender | MessageWithoutSender;

export type GetMessagesResponse = {
  isSuccess: boolean;
  errors: string;
  messages: Message[];
};
