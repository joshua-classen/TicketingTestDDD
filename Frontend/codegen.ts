// import { CodegenConfig } from "@graphql-codegen/cli";
//
// const config: CodegenConfig = {
//   schema: 'http://localhost:5108/graphql/',
//   documents: ['src/**/*.ts'],
//   generates: {
//     './src/__generated__/': {
//       preset: 'client-preset',
//       presetConfig: {
//         gqlTagName: 'gql',
//       },
//     },
//   },
//   // ignoreNoDocuments: true,
// };
//
// export default config;


import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  schema: 'http://localhost:5108/graphql/',
  documents: ['src/**/*.ts'], //graphql dateien geht nicht. Wieso nicht? Das ist doch der Sinn die query in .graphql dateien abzuspeichern?
  // der will hier ts dateien:Plugin "typescript-apollo-angular" validation failed: Plugin "apollo-angular" requires extension to be ".ts"!

    generates: {
    './src/__generated__/': {
      plugins: ['typescript', 'typescript-operations', 'typescript-apollo-angular']
    }
  }
};
export default config;


// todo: mein graphql schema in root ordner erstellen lassen
