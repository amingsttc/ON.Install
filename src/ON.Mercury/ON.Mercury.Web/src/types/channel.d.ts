export type Channel = {
  id: string;
  name: string;
  category: string | undefined;
  description: string | undefined;
};

export type CategoryListEntry = {
  category: string;
  channels: Channel[];
};

export type CategoryList = {
  categories: CategoryListEntry[];
};
