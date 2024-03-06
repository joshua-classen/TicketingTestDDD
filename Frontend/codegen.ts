import { CodegenConfig } from "@graphql-codegen/cli";

const config: CodegenConfig = {
  schema: 'http://localhost:5108/graphql/',
  documents: ['src/**/*.ts'], // Passe den Pfad zu deinen GraphQL-Dokumenten an
  generates: {
    './src/__generated__/': {
      preset: 'client-preset', // Verwende das Angular-Preset????geht noch nicht?
      presetConfig: {
        gqlTagName: 'gql',
      },
    },
  },
  // ignoreNoDocuments: true,
};

export default config;



// todo: mein graphql schema in root ordner erstellen lassen
