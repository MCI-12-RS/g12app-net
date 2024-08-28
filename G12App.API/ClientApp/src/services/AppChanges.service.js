export const GetCommits = async (loginData) => {

  const myHeaders = new Headers();
  myHeaders.append("Accept", "application/vnd.github+json");
  myHeaders.append("Authorization", "Bearer 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");
  myHeaders.append("X-GitHub-Api-Version", "2022-11-28");

  const requestOptions = {
    method: "GET",
    headers: myHeaders,
    redirect: "follow"
  };

  const data = await fetch("https://api.github.com/repos/rafaelalvesdev/g12app-net/commits?sha=master", requestOptions)
                    .then((response) => response.json())
                    .catch((error) => console.error(error));

  return data;
};
