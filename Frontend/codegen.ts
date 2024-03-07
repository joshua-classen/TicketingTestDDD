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
  documents: ['src/**/*.graphql'],
  generates: {
    './graphql/generated.ts': {
      plugins: ['typescript', 'typescript-operations', 'typescript-apollo-angular']
    }
  }
};
export default config;
