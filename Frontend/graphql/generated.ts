import { gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
import * as Apollo from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  UUID: { input: any; output: any; }
  /** The UnsignedInt scalar type represents a unsigned 32-bit numeric non-fractional value greater than or equal to 0. */
  UnsignedInt: { input: any; output: any; }
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER',
  Validation = 'VALIDATION'
}

export type BuyTicketCreateInput = {
  veranstaltungId: Scalars['UUID']['input'];
};

export type KundeCreateInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type KundeLoginInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type KundePayload = {
  __typename?: 'KundePayload';
  email: Scalars['String']['output'];
  jwtToken: Scalars['String']['output'];
};

export type KundeUser = {
  __typename?: 'KundeUser';
  id: Scalars['UUID']['output'];
  purchasedTickets: Array<PurchasedTicket>;
};

export type Mutation = {
  __typename?: 'Mutation';
  /** @deprecated Use MutationPurchaseTicketPaymentIntent instead. */
  buyTicket: PurchasedTicket;
  createKunde: KundePayload;
  createPaymentIntent: StripeClientSecretPayload;
  createVeranstalter: VeranstalterPayload;
  createVeranstaltung: Veranstaltung;
  loginKunde: KundePayload;
  loginVeranstalter: VeranstalterPayload;
};


export type MutationBuyTicketArgs = {
  input: BuyTicketCreateInput;
};


export type MutationCreateKundeArgs = {
  input: KundeCreateInput;
};


export type MutationCreatePaymentIntentArgs = {
  input: BuyTicketCreateInput;
};


export type MutationCreateVeranstalterArgs = {
  input: VeranstalterCreateInput;
};


export type MutationCreateVeranstaltungArgs = {
  input: VeranstaltungCreateInput;
};


export type MutationLoginKundeArgs = {
  input: KundeLoginInput;
};


export type MutationLoginVeranstalterArgs = {
  input: VeranstalterLoginInput;
};

export type PurchasedTicket = {
  __typename?: 'PurchasedTicket';
  id: Scalars['UUID']['output'];
  purchaseDate: Scalars['DateTime']['output'];
  ticketNumber?: Maybe<Scalars['UnsignedInt']['output']>;
  ticketPriceEuroCent?: Maybe<Scalars['UnsignedInt']['output']>;
  veranstaltung: Veranstaltung;
};

export type Query = {
  __typename?: 'Query';
  kundeUser: KundeUser;
  veranstaltungDummy: Veranstaltung;
  veranstaltungen: Array<Veranstaltung>;
};

export type StripeClientSecretPayload = {
  __typename?: 'StripeClientSecretPayload';
  clientSecret: Scalars['String']['output'];
};

export type VeranstalterCreateInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type VeranstalterLoginInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type VeranstalterPayload = {
  __typename?: 'VeranstalterPayload';
  email: Scalars['String']['output'];
  jwtToken: Scalars['String']['output'];
};

export type VeranstalterUser = {
  __typename?: 'VeranstalterUser';
  id: Scalars['UUID']['output'];
  veranstaltungen: Array<Veranstaltung>;
};

export type Veranstaltung = {
  __typename?: 'Veranstaltung';
  id: Scalars['UUID']['output'];
  maxAmountTickets?: Maybe<Scalars['UnsignedInt']['output']>;
  name: Scalars['String']['output'];
  purchasedTickets: Array<PurchasedTicket>;
  ticketPriceEuroCent?: Maybe<Scalars['UnsignedInt']['output']>;
};

export type VeranstaltungCreateInput = {
  maxAmountTickets: Scalars['UnsignedInt']['input'];
  name: Scalars['String']['input'];
  ticketPriceEuroCent: Scalars['UnsignedInt']['input'];
};

export type MyVeranstaltungDummyQueryQueryVariables = Exact<{ [key: string]: never; }>;


export type MyVeranstaltungDummyQueryQuery = { __typename?: 'Query', veranstaltungDummy: { __typename?: 'Veranstaltung', name: string } };

export const MyVeranstaltungDummyQueryDocument = gql`
    query MyVeranstaltungDummyQuery {
  veranstaltungDummy {
    name
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class MyVeranstaltungDummyQueryGQL extends Apollo.Query<MyVeranstaltungDummyQueryQuery, MyVeranstaltungDummyQueryQueryVariables> {
    document = MyVeranstaltungDummyQueryDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }